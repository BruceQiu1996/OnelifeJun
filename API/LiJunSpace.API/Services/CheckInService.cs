using LiJunSpace.API.Channels;
using LiJunSpace.API.Database;
using LiJunSpace.API.Database.Entities;
using LiJunSpace.API.Dtos;
using LiJunSpace.Common.Dtos.Account;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LiJunSpace.API.Services
{
    public class CheckInService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;
        private readonly AddIntegralChannel _addIntegralChannel;

        public CheckInService(JunRecordDbContext junRecordDbContext, AddIntegralChannel addIntegralChannel)
        {
            _junRecordDbContext = junRecordDbContext;
            _addIntegralChannel = addIntegralChannel;
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

            await _junRecordDbContext.CheckInRecords.AddAsync(new CheckInRecord()
            {
                Checker = userId,
                CheckInTime = DateTime.Now,
            });
            await _junRecordDbContext.SaveChangesAsync();


            await _addIntegralChannel.WriteMessageAsync(new Integral()
            {
                Type = IntegralType.Checkin,
                Publisher = userId
            });

            return new ServiceResult();
        }
    }
}
