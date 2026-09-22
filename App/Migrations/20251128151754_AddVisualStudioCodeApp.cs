using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Logger_1._3.Migrations
{
    /// <inheritdoc />
    public partial class AddVisualStudioCodeApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "APPLICATION",
                columns: new[] { "appID", "Category", "IsDefault", "Name", "accountID" },
                values: new object[] { 25, 0, true, "Visual Studio Code", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "APPLICATION",
                keyColumn: "appID",
                keyValue: 25);
        }
    }
}
