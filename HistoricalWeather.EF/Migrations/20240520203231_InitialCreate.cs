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
                    Latitude = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    Elevation = table.Column<decimal>(type: "decimal(5,1)", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationId = table.Column<string>(type: "char(11)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(6,4)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(7,4)", nullable: false),
                    Value = table.Column<string>(type: "char(4)", nullable: false),
                    StartDate = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<int>(type: "int", nullable: false)
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
                name: "WeatherRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationId = table.Column<string>(type: "char(11)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Element = table.Column<string>(type: "char(4)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    MFlag = table.Column<string>(type: "nchar(1)", nullable: false),
                    QFlag = table.Column<string>(type: "nchar(1)", nullable: false),
                    SFlag = table.Column<string>(type: "nchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherRecords_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationDataTypes_StationId",
                table: "StationDataTypes",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherRecords_StationId",
                table: "WeatherRecords",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationDataTypes");

            migrationBuilder.DropTable(
                name: "WeatherRecords");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
