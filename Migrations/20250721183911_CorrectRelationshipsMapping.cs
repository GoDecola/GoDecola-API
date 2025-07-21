using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoDecola.API.Migrations
{
    /// <inheritdoc />
    public partial class CorrectRelationshipsMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Reservations_ReservationId1",
                table: "Guests");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_TravelPackages_TravelPackageId1",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Users_UserId1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TravelPackageId1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Guests_ReservationId1",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "TravelPackageId1",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationId1",
                table: "Guests");

            migrationBuilder.RenameColumn(
                name: "Wifi",
                table: "HotelAmenities",
                newName: "HasWifi");

            migrationBuilder.RenameColumn(
                name: "Restaurant",
                table: "HotelAmenities",
                newName: "HasRestaurant");

            migrationBuilder.RenameColumn(
                name: "Pool",
                table: "HotelAmenities",
                newName: "HasPool");

            migrationBuilder.RenameColumn(
                name: "PetFriendly",
                table: "HotelAmenities",
                newName: "HasPetFriendly");

            migrationBuilder.RenameColumn(
                name: "Parking",
                table: "HotelAmenities",
                newName: "HasParking");

            migrationBuilder.RenameColumn(
                name: "Gym",
                table: "HotelAmenities",
                newName: "HasGym");

            migrationBuilder.RenameColumn(
                name: "BreakfastIncluded",
                table: "HotelAmenities",
                newName: "HasBreakfastIncluded");

            migrationBuilder.RenameColumn(
                name: "AirConditioning",
                table: "HotelAmenities",
                newName: "HasAirConditioning");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasWifi",
                table: "HotelAmenities",
                newName: "Wifi");

            migrationBuilder.RenameColumn(
                name: "HasRestaurant",
                table: "HotelAmenities",
                newName: "Restaurant");

            migrationBuilder.RenameColumn(
                name: "HasPool",
                table: "HotelAmenities",
                newName: "Pool");

            migrationBuilder.RenameColumn(
                name: "HasPetFriendly",
                table: "HotelAmenities",
                newName: "PetFriendly");

            migrationBuilder.RenameColumn(
                name: "HasParking",
                table: "HotelAmenities",
                newName: "Parking");

            migrationBuilder.RenameColumn(
                name: "HasGym",
                table: "HotelAmenities",
                newName: "Gym");

            migrationBuilder.RenameColumn(
                name: "HasBreakfastIncluded",
                table: "HotelAmenities",
                newName: "BreakfastIncluded");

            migrationBuilder.RenameColumn(
                name: "HasAirConditioning",
                table: "HotelAmenities",
                newName: "AirConditioning");

            migrationBuilder.AddColumn<int>(
                name: "TravelPackageId1",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationId1",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TravelPackageId1",
                table: "Reservations",
                column: "TravelPackageId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId1",
                table: "Reservations",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_ReservationId1",
                table: "Guests",
                column: "ReservationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Reservations_ReservationId1",
                table: "Guests",
                column: "ReservationId1",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_TravelPackages_TravelPackageId1",
                table: "Reservations",
                column: "TravelPackageId1",
                principalTable: "TravelPackages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Users_UserId1",
                table: "Reservations",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
