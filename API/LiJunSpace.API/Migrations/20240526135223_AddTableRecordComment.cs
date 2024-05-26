using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTableRecordComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Content = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Publisher = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    RecordId = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    PublishTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Accounts_Publisher",
                        column: x => x.Publisher,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                column: "Password",
                value: "U0OvcpzVoz9maY7ABB1uCA==.dZpZDm22bnXmOCqn85oyHw8vDWemnHVe7t7n7Xw7IeA=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "ggNPwzzJfiUPXe5K5oo1UQ==.upk6HcZlsIa2hufr0BwwYai3Ooyhcrg4iS9DcOVsBQ0=");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Publisher",
                table: "Comments",
                column: "Publisher");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RecordId",
                table: "Comments",
                column: "RecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                column: "Password",
                value: "fHY6mVWGfkoU9I/vErym7Q==.TGZjE/nLWAdVw0KpeOzpabDTkxM48kFe2otcyl4KLHY=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "4BVQ7QSY1721V5LkErxQUQ==.Tg6Oj+rMrghIRq+6b6bA5Ighc6vhmcP9Ot5IGRqgr2Y=");
        }
    }
}
