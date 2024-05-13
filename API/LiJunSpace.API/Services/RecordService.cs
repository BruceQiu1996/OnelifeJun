using LiJunSpace.API.Database;
using LiJunSpace.API.Database.Entities;
using LiJunSpace.API.Dtos;
using LiJunSpace.Common.Dtos.Record;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            if (recordQueryDto.TimeDesc)
            {
                query.Skip(10 * (recordQueryDto.Page - 1)).Take(10).OrderByDescending(x => x.PublishTime);
            }
            else
            {
                query.Skip(10 * (recordQueryDto.Page - 1)).Take(10).OrderBy(x => x.PublishTime);
            }

            RecordQueryResultDto result = new RecordQueryResultDto();
            var datas = await query.Include(x => x.Account).ToListAsync();
            List<RecordDto> records = new List<RecordDto>();
            foreach (var item in datas)
            {
                var dto = item.ToDto();
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
