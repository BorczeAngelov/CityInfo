using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations
{
    public partial class RenameCitiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterests_Cties_CityId",
                table: "PointOfInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cties",
                table: "Cties");

            migrationBuilder.RenameTable(
                name: "Cties",
                newName: "Cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterests_Cities_CityId",
                table: "PointOfInterests",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterests_Cities_CityId",
                table: "PointOfInterests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "Cties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cties",
                table: "Cties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterests_Cties_CityId",
                table: "PointOfInterests",
                column: "CityId",
                principalTable: "Cties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
