using LiJunSpace.API.Database.Entities;
using LiJunSpace.API.Services;
using System.Threading.Channels;

namespace LiJunSpace.API.Channels
{
    public class AddIntegralChannel
    {
        private readonly ChannelWriter<Integral> _writeChannel;
        private readonly ChannelReader<Integral> _readChannel;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IServiceProvider _provider;

        public AddIntegralChannel(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            var channel = Channel.CreateUnbounded<Integral>();
            _writeChannel = channel.Writer;
            _readChannel = channel.Reader;
            IntegralCustomer readOperateLogService = new IntegralCustomer(_readChannel, _provider.GetRequiredService<ILogger<IntegralCustomer>>(), configuration,
                _provider.GetRequiredService<IntegralService>());

            Task.Run(async () => await readOperateLogService.StartAsync(_cancellationTokenSource.Token));
        }

        ~AddIntegralChannel()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async Task WriteMessageAsync(Integral integral)
        {
            await _writeChannel.WriteAsync(integral);
        }

        public class IntegralCustomer
        {
            private readonly ChannelReader<Integral> _readChannel;
            private readonly ILogger<IntegralCustomer> _logger;
            private readonly IConfiguration _configuration;
            private readonly IntegralService _integralService;

            public IntegralCustomer(ChannelReader<Integral> readChannel,
                                   ILogger<IntegralCustomer> logger,
                                   IConfiguration configuration,
                                   IntegralService integralService)
            {
                _logger = logger;
                _readChannel = readChannel;
                _configuration = configuration;
                _integralService = integralService;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                while (await _readChannel.WaitToReadAsync(cancellationToken))
                {
                    while (_readChannel.TryRead(out var obj))
                    {
                        try
                        {
                            await _integralService.InsertNewIntegralAsync(obj);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
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
        }
    }
}
