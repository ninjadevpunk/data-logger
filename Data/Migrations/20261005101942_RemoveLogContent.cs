using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLogContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_CHECKLIST_ChecklistlogID",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "LOG");

            migrationBuilder.AlterColumn<int>(
                name: "ChecklistlogID",
                table: "Item",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "ACCOUNT",
                keyColumn: "accountID",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "support@datalogger.space", "" });

            migrationBuilder.UpdateData(
                table: "PROJECT",
                keyColumn: "projectID",
                keyValue: 1,
                column: "Name",
                value: "Unnamed Project");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_CHECKLIST_ChecklistlogID",
                table: "Item",
                column: "ChecklistlogID",
                principalTable: "CHECKLIST",
                principalColumn: "logID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_CHECKLIST_ChecklistlogID",
                table: "Item");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "LOG",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ChecklistlogID",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ACCOUNT",
                keyColumn: "accountID",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "support@datalogger.co.za", "pcsx2024" });

            migrationBuilder.UpdateData(
                table: "PROJECT",
                keyColumn: "projectID",
                keyValue: 1,
                column: "Name",
                value: "Unknown");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_CHECKLIST_ChecklistlogID",
                table: "Item",
                column: "ChecklistlogID",
                principalTable: "CHECKLIST",
                principalColumn: "logID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
