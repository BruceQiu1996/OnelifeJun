using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    UserName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    DisplayName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Password = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Signature = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Sex = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Title = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Content = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Publisher = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    PublishTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Images = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Accounts_Publisher",
                        column: x => x.Publisher,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Avatar", "Birthday", "CreateTime", "DisplayName", "Password", "Sex", "Signature", "UpdateTime", "UserName" },
                values: new object[,]
                {
                    { "09B4A455-BE83-559B-9102-A5D61094CBC6", null, new DateOnly(1995, 8, 24), new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "仇伟", "Sp1W8wCyeejyhrTQMczB4Q==.mrZS5hDX7yXzh5izfRd/cmhF/YzC5WxT7oJdWPu+eR4=", true, null, new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "qw" },
                    { "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0", null, new DateOnly(1994, 1, 12), new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "王丽君", "k4f2MVwvNGYTqmNZ4dgEyg==.kRfR7yVLjwMZA2xWn7FguJLVj7fqP2NsgNhoXrJBWQ0=", false, null, new DateTime(2024, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "wlj" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_Publisher",
                table: "Records",
                column: "Publisher");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
