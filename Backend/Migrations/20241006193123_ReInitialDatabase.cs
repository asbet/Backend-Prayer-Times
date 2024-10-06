using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class ReInitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrayerTimings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fajr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sunrise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dhuhr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Asr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sunset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maghrib = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imsak = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Midnight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GregorianDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    HijriDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrayerTimings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrayerTimings_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrayerTimings_CityId",
                table: "PrayerTimings",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrayerTimings");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
