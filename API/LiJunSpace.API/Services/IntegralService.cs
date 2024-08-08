using LiJunSpace.API.Database;
using LiJunSpace.API.Database.Entities;

namespace LiJunSpace.API.Services
{
    public class IntegralService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;
        public IntegralService(JunRecordDbContext junRecordDbContext)
        {
            _junRecordDbContext = junRecordDbContext;
        }

        public async Task InsertNewIntegralAsync(Integral integral) 
        {
            await _junRecordDbContext.Integrals.AddAsync(integral);
            await _junRecordDbContext.SaveChangesAsync();
        }
    }
}
