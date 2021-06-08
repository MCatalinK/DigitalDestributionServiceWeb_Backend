using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalDistribution.Models.Database.Migrations
{
    public partial class ChangedSomeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LibraryItems",
                table: "LibraryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LibraryItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LibraryItems",
                table: "LibraryItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryItems_UserId",
                table: "LibraryItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LibraryItems",
                table: "LibraryItems");

            migrationBuilder.DropIndex(
                name: "IX_LibraryItems_UserId",
                table: "LibraryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "InvoiceItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LibraryItems",
                table: "LibraryItems",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems",
                columns: new[] { "InvoiceId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
