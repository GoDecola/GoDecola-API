using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoDecola.API.Migrations
{
    /// <inheritdoc />
    public partial class UnifyTravelPackageMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelPackageImages");

            migrationBuilder.DropTable(
                name: "TravelPackageVideos");

            migrationBuilder.CreateTable(
                name: "TravelPackageMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPackageId = table.Column<int>(type: "int", nullable: false),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPackageMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelPackageMedias_TravelPackages_TravelPackageId",
                        column: x => x.TravelPackageId,
                        principalTable: "TravelPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelPackageMedias_TravelPackageId",
                table: "TravelPackageMedias",
                column: "TravelPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelPackageMedias");

            migrationBuilder.CreateTable(
                name: "TravelPackageImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPackageId = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPackageImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelPackageImages_TravelPackages_TravelPackageId",
                        column: x => x.TravelPackageId,
                        principalTable: "TravelPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TravelPackageVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelPackageId = table.Column<int>(type: "int", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelPackageVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelPackageVideos_TravelPackages_TravelPackageId",
                        column: x => x.TravelPackageId,
                        principalTable: "TravelPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelPackageImages_TravelPackageId",
                table: "TravelPackageImages",
                column: "TravelPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelPackageVideos_TravelPackageId",
                table: "TravelPackageVideos",
                column: "TravelPackageId");
        }
    }
}
