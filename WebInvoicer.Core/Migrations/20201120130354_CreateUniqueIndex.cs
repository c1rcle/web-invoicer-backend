using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoicer.Core.Migrations
{
    public partial class CreateUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number",
                table: "Invoices",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_Number",
                table: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Invoices",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
