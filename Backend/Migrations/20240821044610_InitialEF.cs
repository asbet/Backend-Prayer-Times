using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialEF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarByCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarByCities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expanded = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Month",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    En = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ar = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Month", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OffesetId = table.Column<int>(type: "int", nullable: false),
                    Imsak = table.Column<int>(type: "int", nullable: false),
                    Fajr = table.Column<int>(type: "int", nullable: false),
                    Sunrise = table.Column<int>(type: "int", nullable: false),
                    Dhuhr = table.Column<int>(type: "int", nullable: false),
                    Asr = table.Column<int>(type: "int", nullable: false),
                    Maghrib = table.Column<int>(type: "int", nullable: false),
                    Sunset = table.Column<int>(type: "int", nullable: false),
                    Isha = table.Column<int>(type: "int", nullable: false),
                    Midnight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Params",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fajr = table.Column<int>(type: "int", nullable: false),
                    Isha = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Params", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weekday",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    En = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ar = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weekday", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Datum",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalendarByCityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Datum_CalendarByCities_CalendarByCityId",
                        column: x => x.CalendarByCityId,
                        principalTable: "CalendarByCities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Method",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParamsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Method", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Method_Params_ParamsId",
                        column: x => x.ParamsId,
                        principalTable: "Params",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gregorian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekdayId = table.Column<int>(type: "int", nullable: false),
                    MonthId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gregorian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gregorian_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gregorian_Month_MonthId",
                        column: x => x.MonthId,
                        principalTable: "Month",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gregorian_Weekday_WeekdayId",
                        column: x => x.WeekdayId,
                        principalTable: "Weekday",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hijri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HijriId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekdayId = table.Column<int>(type: "int", nullable: false),
                    MonthId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    Holidays = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hijri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hijri_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hijri_Month_MonthId",
                        column: x => x.MonthId,
                        principalTable: "Month",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hijri_Weekday_WeekdayId",
                        column: x => x.WeekdayId,
                        principalTable: "Weekday",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimingsId = table.Column<int>(type: "int", nullable: false),
                    Fajr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sunrise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dhuhr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Asr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sunset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maghrib = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imsak = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Midnight = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timings_Datum_TimingsId",
                        column: x => x.TimingsId,
                        principalTable: "Datum",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetaId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MethodId = table.Column<int>(type: "int", nullable: false),
                    LatitudeAdjustmentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MidnightMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    School = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OffsetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metas_Datum_MetaId",
                        column: x => x.MetaId,
                        principalTable: "Datum",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Metas_Method_MethodId",
                        column: x => x.MethodId,
                        principalTable: "Method",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metas_Offset_OffsetId",
                        column: x => x.OffsetId,
                        principalTable: "Offset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Readable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GregorianId = table.Column<int>(type: "int", nullable: false),
                    HijriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dates_Datum_DateId",
                        column: x => x.DateId,
                        principalTable: "Datum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dates_Gregorian_GregorianId",
                        column: x => x.GregorianId,
                        principalTable: "Gregorian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dates_Hijri_HijriId",
                        column: x => x.HijriId,
                        principalTable: "Hijri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dates_DateId",
                table: "Dates",
                column: "DateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dates_GregorianId",
                table: "Dates",
                column: "GregorianId");

            migrationBuilder.CreateIndex(
                name: "IX_Dates_HijriId",
                table: "Dates",
                column: "HijriId");

            migrationBuilder.CreateIndex(
                name: "IX_Datum_CalendarByCityId",
                table: "Datum",
                column: "CalendarByCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Gregorian_DesignationId",
                table: "Gregorian",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Gregorian_MonthId",
                table: "Gregorian",
                column: "MonthId");

            migrationBuilder.CreateIndex(
                name: "IX_Gregorian_WeekdayId",
                table: "Gregorian",
                column: "WeekdayId");

            migrationBuilder.CreateIndex(
                name: "IX_Hijri_DesignationId",
                table: "Hijri",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Hijri_MonthId",
                table: "Hijri",
                column: "MonthId");

            migrationBuilder.CreateIndex(
                name: "IX_Hijri_WeekdayId",
                table: "Hijri",
                column: "WeekdayId");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_MetaId",
                table: "Metas",
                column: "MetaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metas_MethodId",
                table: "Metas",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Metas_OffsetId",
                table: "Metas",
                column: "OffsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Method_ParamsId",
                table: "Method",
                column: "ParamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Timings_TimingsId",
                table: "Timings",
                column: "TimingsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dates");

            migrationBuilder.DropTable(
                name: "Metas");

            migrationBuilder.DropTable(
                name: "Timings");

            migrationBuilder.DropTable(
                name: "Gregorian");

            migrationBuilder.DropTable(
                name: "Hijri");

            migrationBuilder.DropTable(
                name: "Method");

            migrationBuilder.DropTable(
                name: "Offset");

            migrationBuilder.DropTable(
                name: "Datum");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropTable(
                name: "Month");

            migrationBuilder.DropTable(
                name: "Weekday");

            migrationBuilder.DropTable(
                name: "Params");

            migrationBuilder.DropTable(
                name: "CalendarByCities");
        }
    }
}
