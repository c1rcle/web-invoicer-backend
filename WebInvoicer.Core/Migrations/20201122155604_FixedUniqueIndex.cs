using Microsoft.EntityFrameworkCore.Migrations;

namespace WebInvoicer.Core.Migrations
{
    public partial class FixedUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_Number",
                table: "Invoices");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number_UserId",
                table: "Invoices",
                columns: new[] { "Number", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_Number_UserId",
                table: "Invoices");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number",
                table: "Invoices",
                column: "Number",
                unique: true);
        }
    }
}
