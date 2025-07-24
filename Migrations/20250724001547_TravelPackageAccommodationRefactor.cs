using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoDecola.API.Migrations
{
    /// <inheritdoc />
    public partial class TravelPackageAccommodationRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelAmenities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "TravelPackages");

            migrationBuilder.DropColumn(
                name: "NumberBaths",
                table: "TravelPackages");

            migrationBuilder.RenameColumn(
                name: "NumberBeds",
                table: "TravelPackages",
                newName: "PackageType");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                table: "TravelPackages",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TravelPackages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotionEndDate",
                table: "TravelPackages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PromotionStartDate",
                table: "TravelPackages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPackageId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    NumberBaths = table.Column<int>(type: "int", nullable: false),
                    NumberBeds = table.Column<int>(type: "int", nullable: false),
                    HasWifi = table.Column<bool>(type: "bit", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false),
                    HasPool = table.Column<bool>(type: "bit", nullable: false),
                    HasGym = table.Column<bool>(type: "bit", nullable: false),
                    HasRestaurant = table.Column<bool>(type: "bit", nullable: false),
                    HasPetFriendly = table.Column<bool>(type: "bit", nullable: false),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    HasBreakfastIncluded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccommodationDetails_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodationDetails_TravelPackages_TravelPackageId",
                        column: x => x.TravelPackageId,
                        principalTable: "TravelPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationDetails_AddressId",
                table: "AccommodationDetails",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationDetails_TravelPackageId",
                table: "AccommodationDetails",
                column: "TravelPackageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationDetails");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "TravelPackages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TravelPackages");

            migrationBuilder.DropColumn(
                name: "PromotionEndDate",
                table: "TravelPackages");

            migrationBuilder.DropColumn(
                name: "PromotionStartDate",
                table: "TravelPackages");

            migrationBuilder.DropColumn(
                name: "CheckInDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CheckOutDate",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "PackageType",
                table: "TravelPackages",
                newName: "NumberBeds");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "TravelPackages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberBaths",
                table: "TravelPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HotelAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    HasBreakfastIncluded = table.Column<bool>(type: "bit", nullable: false),
                    HasGym = table.Column<bool>(type: "bit", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false),
                    HasPetFriendly = table.Column<bool>(type: "bit", nullable: false),
                    HasPool = table.Column<bool>(type: "bit", nullable: false),
                    HasRestaurant = table.Column<bool>(type: "bit", nullable: false),
                    HasWifi = table.Column<bool>(type: "bit", nullable: false),
                    TravelPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelAmenities_TravelPackages_TravelPackageId",
                        column: x => x.TravelPackageId,
                        principalTable: "TravelPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenities_TravelPackageId",
                table: "HotelAmenities",
                column: "TravelPackageId",
                unique: true);
        }
    }
}
