using LiJunSpace.API.Database.Entiies;
using LiJunSpace.API.Database.Entities;
using LiJunSpace.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace LiJunSpace.API.Database
{
    public class JunRecordDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<OurEvent> Events { get; set; }
        public DbSet<CheckInRecord> CheckInRecords { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Integral> Integrals { get; set; }
        public DbSet<LikeRecord> LikeRecords { get; set; }

        private readonly PasswordHelper _passwordHelper;
        public JunRecordDbContext(DbContextOptions options, PasswordHelper passwordHelper) : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
            _passwordHelper = passwordHelper;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4 ");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasData(new Account()
            {
                Id = "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                DisplayName = "王丽君",
                UserName = "wlj",
                Birthday = DateOnly.Parse("1994.01.12"),
                Sex = false,
                Password = _passwordHelper.HashPassword("123456"),
                CreateTime = DateTime.Parse("2024-05-10"),
                UpdateTime = DateTime.Parse("2024-05-10"),
            },
            new Account()
            {
                Id = "09B4A455-BE83-559B-9102-A5D61094CBC6",
                DisplayName = "仇伟",
                UserName = "qw",
                Birthday = DateOnly.Parse("1995.08.24"),
                Sex = true,
                Password = _passwordHelper.HashPassword("123456"),
                CreateTime = DateTime.Parse("2024-05-10"),
                UpdateTime = DateTime.Parse("2024-05-10"),
            });
        }
    }
}
