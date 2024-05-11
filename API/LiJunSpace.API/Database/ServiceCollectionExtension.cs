using LiJunSpace.API.Database.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LiJunSpace.API.Database
{
    public static class ServiceCollectionExtension
    {
        public static void AddEfCoreContext(this IServiceCollection services, IConfiguration configuration)
        {
            var mysqlConfig = configuration.GetSection("Mysql").Get<MysqlOptions>();
            var serverVersion = new MariaDbServerVersion(new Version(8, 0, 29));
            services.AddDbContext<DbContext, JunRecordDbContext>(options =>
            {
                options.UseMySql(mysqlConfig.ConnectionString, serverVersion, optionsBuilder =>
                {
                    optionsBuilder.MinBatchSize(4).MigrationsAssembly("LiJunSpace.API")
                                                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });
        }
    }
}
