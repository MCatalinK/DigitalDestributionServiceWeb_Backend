using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalDistribution.Models.Database.Migrations
{
    public partial class AddedVersionFieldToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Version",
                table: "Updates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Version",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Updates");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Products");
        }
    }
}
