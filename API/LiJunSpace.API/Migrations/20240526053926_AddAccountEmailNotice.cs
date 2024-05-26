using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiJunSpace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountEmailNotice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OpenEmailNotice",
                table: "Accounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                columns: new[] { "OpenEmailNotice", "Password" },
                values: new object[] { false, "Zd7odcf/xe4TE3gcPfeIuw==.jDpURCb6bGyJOueQa2L7YY0Cz/EBJMrv/7UA6aWee0s=" });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                columns: new[] { "OpenEmailNotice", "Password" },
                values: new object[] { false, "/ga1bnmANG2t1JkQOJCx5g==.+SOgHsO8TW8ziUC+67s7TJYunuX3ROHEl8P/4DVtZrU=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenEmailNotice",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "09B4A455-BE83-559B-9102-A5D61094CBC6",
                column: "Password",
                value: "If4e5mBbWxEE2FfccjOkXQ==.L1NzaSKBUdZoHtIQ1kO56/2ah0IiOL7s2GHzRNgdWso=");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "D9006BC9-80C6-5AC7-62EF-DC73F2FF17F0",
                column: "Password",
                value: "Gyjm6J2tS45OxtWutTdkQA==.zaydGmdksJm9Zj1l4wsZqPeZ8zoO5sCNtzWFSjYNOvM=");
        }
    }
}
