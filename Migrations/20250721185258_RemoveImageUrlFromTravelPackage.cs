using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoDecola.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageUrlFromTravelPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TravelPackages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TravelPackages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
