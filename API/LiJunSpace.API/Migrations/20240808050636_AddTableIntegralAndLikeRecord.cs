using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTableIntegralAndLikeRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Integrals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Publisher = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Integrals_Accounts_Publisher",
                        column: x => x.Publisher,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "LikeRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Activer = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    Passiver = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    LikeTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeRecords_Accounts_Activer",
                        column: x => x.Activer,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikeRecords_Accounts_Passiver",
                        column: x => x.Passiver,
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
                value: "HBIH7dGSJCaATqrickjf/g==.g+41VZi2yBpR5SrlZ0b5qh18+M5E1yPnoLznSz8dpUM=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "FddKgJa6OWqmlIYq3ZxObQ==.ZJWP7tODXs+Hv0yRxNJ6iqwOcla2ysjULvJ0pgy50p4=");

            migrationBuilder.CreateIndex(
                name: "IX_Integrals_Publisher",
                table: "Integrals",
                column: "Publisher");

            migrationBuilder.CreateIndex(
                name: "IX_LikeRecords_Activer",
                table: "LikeRecords",
                column: "Activer");

            migrationBuilder.CreateIndex(
                name: "IX_LikeRecords_Passiver",
                table: "LikeRecords",
                column: "Passiver");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Integrals");

            migrationBuilder.DropTable(
                name: "LikeRecords");

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
        }
    }
}
