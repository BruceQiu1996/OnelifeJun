using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCheckInRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckInRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Checker = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    CheckInTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckInRecords_Accounts_Checker",
                        column: x => x.Checker,
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
                value: "fHY6mVWGfkoU9I/vErym7Q==.TGZjE/nLWAdVw0KpeOzpabDTkxM48kFe2otcyl4KLHY=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "4BVQ7QSY1721V5LkErxQUQ==.Tg6Oj+rMrghIRq+6b6bA5Ighc6vhmcP9Ot5IGRqgr2Y=");

            migrationBuilder.CreateIndex(
                name: "IX_CheckInRecords_Checker",
                table: "CheckInRecords",
                column: "Checker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckInRecords");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                column: "Password",
                value: "Zd7odcf/xe4TE3gcPfeIuw==.jDpURCb6bGyJOueQa2L7YY0Cz/EBJMrv/7UA6aWee0s=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "/ga1bnmANG2t1JkQOJCx5g==.+SOgHsO8TW8ziUC+67s7TJYunuX3ROHEl8P/4DVtZrU=");
        }
    }
}
