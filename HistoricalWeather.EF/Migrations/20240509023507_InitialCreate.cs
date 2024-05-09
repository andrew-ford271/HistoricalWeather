using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HistoricalWeather.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(11)", nullable: false),
                    Latitude = table.Column<double>(type: "decimal(2,4)", nullable: false),
                    Longitude = table.Column<double>(type: "decimal(3,4)", nullable: false),
                    Elevation = table.Column<double>(type: "decimal(4,1)", nullable: false),
                    State = table.Column<string>(type: "char(2)", nullable: true),
                    StationName = table.Column<string>(type: "char(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StationDataTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StationId = table.Column<string>(type: "char(11)", nullable: false),
                    Latitude = table.Column<double>(type: "decimal(2,4)", nullable: false),
                    Longitude = table.Column<double>(type: "decimal(3,4)", nullable: false),
                    Value = table.Column<string>(type: "char(4)", nullable: false),
                    StartDate = table.Column<int>(type: "INTEGER", nullable: false),
                    EndDate = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationDataTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationDataTypes_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherRecordMonths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StationId = table.Column<string>(type: "char(11)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Element = table.Column<string>(type: "char(4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecordMonths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherRecordMonths_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherRecordDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(type: "char(4)", nullable: false),
                    MFlag = table.Column<char>(type: "char(1)", nullable: false),
                    QFlag = table.Column<char>(type: "char(1)", nullable: false),
                    SFlag = table.Column<char>(type: "char(1)", nullable: false),
                    WeatherRecordMonthId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecordDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherRecordDays_WeatherRecordMonths_WeatherRecordMonthId",
                        column: x => x.WeatherRecordMonthId,
                        principalTable: "WeatherRecordMonths",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationDataTypes_StationId",
                table: "StationDataTypes",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecordDays_WeatherRecordMonthId",
                table: "WeatherRecordDays",
                column: "WeatherRecordMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecordMonths_StationId",
                table: "WeatherRecordMonths",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationDataTypes");

            migrationBuilder.DropTable(
                name: "WeatherRecordDays");

            migrationBuilder.DropTable(
                name: "WeatherRecordMonths");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
