using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalDistribution.Models.Database.Migrations
{
    public partial class ChangedDeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Profiles_ProfileId",
                table: "Reviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
