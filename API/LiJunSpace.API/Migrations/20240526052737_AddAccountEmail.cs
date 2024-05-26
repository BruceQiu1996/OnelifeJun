using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                columns: new[] { "Email", "Password" },
                values: new object[] { null, "If4e5mBbWxEE2FfccjOkXQ==.L1NzaSKBUdZoHtIQ1kO56/2ah0IiOL7s2GHzRNgdWso=" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                columns: new[] { "Email", "Password" },
                values: new object[] { null, "Gyjm6J2tS45OxtWutTdkQA==.zaydGmdksJm9Zj1l4wsZqPeZ8zoO5sCNtzWFSjYNOvM=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Accounts");

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
        }
    }
}
