using LiJunSpace.API.Database;
using LiJunSpace.API.Database.Entities;
using LiJunSpace.API.Dtos;
using LiJunSpace.Common.Dtos.Record;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace LiJunSpace.API.Services
{
    public class RecordService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;
        private readonly IConfiguration _configuration;
        public RecordService(JunRecordDbContext junRecordDbContext, IConfiguration configuration)
        {
            _junRecordDbContext = junRecordDbContext;
            _configuration = configuration;
        }

        public async Task<ServiceResult<string>> UploadRecordImageAsync(string userId, IFormFile file)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return new ServiceResult<string>(HttpStatusCode.BadRequest, "用户异常");
            }

            if (file.Length > 1024 * 1024 * 10)
            {
                return new ServiceResult<string>(HttpStatusCode.BadRequest, "图片大小不符要求");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var saveFolder = Path.Combine(_configuration.GetSection("FileStorage:RecordImagesLocation").Value!, $"user-{userId}");
            var fileName = $"{Path.GetRandomFileName()}{fileExtension}";
            var fullName = Path.Combine(saveFolder, fileName);

            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            using (var fs = File.Create(fullName))
            {
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
            }

            return new ServiceResult<string>(fileName);
        }

        public async Task<ServiceResult> CreateNewRecordAsync(RecordCreationDto recordCreationDto, string userId)
        {
            var entity = recordCreationDto.ToRecordEntity(userId);
            if (!string.IsNullOrEmpty(entity.Images))
            {
                var imageItems = JsonSerializer.Deserialize<IEnumerable<string>>(entity.Images);
                foreach (var item in imageItems)
                {
                    var path = Path.Combine(_configuration.GetSection("FileStorage:RecordImagesLocation").Value!, $"user-{userId}", item);
                    if (!File.Exists(path))
                        throw new InvalidOperationException("Can't find Image Location");
                }
            }
            _junRecordDbContext.Records.Add(entity);
            await _junRecordDbContext.SaveChangesAsync();

            return new ServiceResult();
        }

        public async Task<ServiceResult<RecordQueryResultDto>> GetByPageAsync(RecordQueryDto recordQueryDto)
        {
            IQueryable<Record> query = _junRecordDbContext.Records.AsQueryable();
            if (!string.IsNullOrEmpty(recordQueryDto.Key))
            {
                query = _junRecordDbContext.Records
                    .Where(x => (!string.IsNullOrEmpty(x.Title) && x.Title.Contains(recordQueryDto.Key))
                    || (!string.IsNullOrEmpty(x.Content) && x.Content.Contains(recordQueryDto.Key)));
            }

            var allCount = await query.CountAsync();

            RecordQueryResultDto result = new RecordQueryResultDto();
            var datas = await query
                .Include(x => x.Account).OrderByDescending(x => x.PublishTime).Skip(10 * (recordQueryDto.Page - 1)).Take(10).ToListAsync();
            List<RecordDto> records = new List<RecordDto>();
            foreach (var item in datas)
            {
                var dto = item.ToDto();
                if (!string.IsNullOrEmpty(item.Images))
                {
                    JsonSerializer.Deserialize<List<string>>(item.Images).ForEach(x =>
                    {
                        dto.Images.Add($"user-{dto.PublisherId}/{x}");
                    });
                }
                dto.publisherAvatar = item.Account.Avatar;
                if (string.IsNullOrEmpty(dto.publisherAvatar))
                {
                    dto.publisherAvatar = item.Account.Sex ? "default1.jpg" : "default0.jpg";
                }

                records.Add(dto);
            }
            result.Records = records;
            result.AllCount = allCount;

            return new ServiceResult<RecordQueryResultDto>(result);
        }
    }
}
