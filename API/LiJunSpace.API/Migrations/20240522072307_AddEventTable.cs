using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    Desc = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UseSeconds = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Publisher = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ShowOnMainpage = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Accounts_Publisher",
                        column: x => x.Publisher,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                column: "Password",
                value: "PbO913GwkqqpF264Z1LcKA==.cSlk++GJgJoZ7yrIfxtIszLfvQvYL9stm7uLEb8TJe8=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "1os3whbbibTwaXj7e1gCcA==.V1Y8vcuu024ymakdpHcFUVXQkyFwXWnH950BkBg/rbQ=");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Publisher",
                table: "Events",
                column: "Publisher");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                column: "Password",
                value: "Sp1W8wCyeejyhrTQMczB4Q==.mrZS5hDX7yXzh5izfRd/cmhF/YzC5WxT7oJdWPu+eR4=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "k4f2MVwvNGYTqmNZ4dgEyg==.kRfR7yVLjwMZA2xWn7FguJLVj7fqP2NsgNhoXrJBWQ0=");
        }
    }
}
