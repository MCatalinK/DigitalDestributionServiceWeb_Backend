using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalDistribution.Models.Database.Migrations
{
    public partial class ChangedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_UserId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Licence",
                table: "InvoiceItems");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Invoices",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                newName: "IX_Invoices_AddressId");

            migrationBuilder.AddColumn<string>(
                name: "DownloadLink",
                table: "LibraryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Addresses_AddressId",
                table: "Invoices",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Addresses_AddressId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DownloadLink",
                table: "LibraryItems");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Invoices",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_AddressId",
                table: "Invoices",
                newName: "IX_Invoices_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Licence",
                table: "InvoiceItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_UserId",
                table: "Invoices",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
