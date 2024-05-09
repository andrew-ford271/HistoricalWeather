using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HistoricalWeather.EF.Migrations
{
    /// <inheritdoc />
    public partial class DayColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "WeatherRecordDays",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "WeatherRecordDays");
        }
    }
}
