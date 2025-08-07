using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoDecola.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEditedColumnToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Wishlists_WishlistId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_WishlistId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Wishlists");

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "WishlistId",
                table: "Wishlists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_WishlistId",
                table: "Wishlists",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Wishlists_WishlistId",
                table: "Wishlists",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id");
        }
    }
}
