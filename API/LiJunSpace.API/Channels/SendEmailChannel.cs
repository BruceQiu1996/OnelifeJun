using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Channels;

namespace LiJunSpace.API.Channels
{
    public class SendEmailObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
    }

    public class SendEmailChannel
    {
        private readonly ChannelWriter<SendEmailObject> _writeChannel;
        private readonly ChannelReader<SendEmailObject> _readChannel;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IServiceProvider _provider;

        public SendEmailChannel(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            var channel = Channel.CreateUnbounded<SendEmailObject>();
            _writeChannel = channel.Writer;
            _readChannel = channel.Reader;
            MessageCustomer readOperateLogService = new MessageCustomer(_readChannel, _provider.GetRequiredService<ILogger<MessageCustomer>>(), configuration);

            Task.Run(async () => await readOperateLogService.StartAsync(_cancellationTokenSource.Token));
        }

        ~SendEmailChannel()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async Task WriteMessageAsync(IEnumerable<SendEmailObject> objects)
        {
            foreach (var obj in objects)
            {
                await _writeChannel.WriteAsync(obj);
            }
        }

        public class MessageCustomer
        {
            private readonly ChannelReader<SendEmailObject> _readChannel;
            private readonly ILogger<MessageCustomer> _logger;
            private readonly IConfiguration _configuration;

            public MessageCustomer(ChannelReader<SendEmailObject> readChannel,
                                   ILogger<MessageCustomer> logger,
                                   IConfiguration configuration)
            {
                _logger = logger;
                _readChannel = readChannel;
                _configuration = configuration;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                while (await _readChannel.WaitToReadAsync(cancellationToken))
                {
                    while (_readChannel.TryRead(out var obj))
                    {
                        try
                        {
                            await SendEmailAsync(obj.Target, obj.Title, obj.Content);
                        }
                        catch (SmtpException ex)
                        {
                            _logger.LogError(ex.StatusCode.ToString());
                            continue;
                        }
                    }

                    if (cancellationToken.IsCancellationRequested) break;
                }
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            /// <summary>
            /// 发送邮件
            /// </summary>
            /// <param name="mailTo">发送人</param>
            /// <param name="mailTitle">邮件标题</param>
            /// <param name="mailContent">邮件内容</param>
            /// <returns></returns>
            public async Task SendEmailAsync(string mailTo, string mailTitle, string mailContent)
            {
                string stmpServer = @"smtp.qq.com";
                string mailAccount = _configuration.GetSection("Email:Account").Value!;//邮箱账号
                string pwd = _configuration.GetSection("Email:Token").Value!;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
                smtpClient.Host = stmpServer;//指定发送方SMTP服务器
                smtpClient.EnableSsl = false;//使用安全加密连接
                smtpClient.UseDefaultCredentials = false;//不和请求一起发送
                smtpClient.Credentials = new NetworkCredential(mailAccount, pwd);//设置发送账号密码

                MailMessage mailMessage = new MailMessage(mailAccount, mailTo);//实例化邮件信息实体并设置发送方和接收方
                mailMessage.Subject = mailTitle;//设置发送邮件得标题
                mailMessage.Body = mailContent;//设置发送邮件内容
                mailMessage.BodyEncoding = Encoding.UTF8;//设置发送邮件得编码
                mailMessage.IsBodyHtml = false;//设置标题是否为HTML格式
                mailMessage.Priority = MailPriority.Normal;//设置邮件发送优先级

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);//发送邮件
                }
                catch (SmtpException ex)
                {
                    throw ex;
                }
            }
        }
    }
}
