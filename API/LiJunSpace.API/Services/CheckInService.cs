using LiJunSpace.API.Database;
using LiJunSpace.API.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LiJunSpace.API.Services
{
    public class CheckInService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;

        public CheckInService(JunRecordDbContext junRecordDbContext)
        {
            _junRecordDbContext = junRecordDbContext;
        }

        /// <summary>
        /// 账号签到
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult> CheckInAsync(string userId)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new ServiceResult(HttpStatusCode.BadRequest, "用户异常");

            var record = await _junRecordDbContext.CheckInRecords.FirstOrDefaultAsync(x => x.Checker == userId &&
                x.CheckInTime.Year == DateTime.Now.Year &&
                x.CheckInTime.Month == DateTime.Now.Month &&
                x.CheckInTime.Day == DateTime.Now.Day);

            if (record != null)
            {
                return new ServiceResult(HttpStatusCode.BadRequest, "签到异常");
            }

            await _junRecordDbContext.CheckInRecords.AddAsync(new Database.Entities.CheckInRecord()
            {
                Checker = userId,
                CheckInTime = DateTime.Now,
            });

            await _junRecordDbContext.SaveChangesAsync();

            return new ServiceResult();
        }
    }
}
