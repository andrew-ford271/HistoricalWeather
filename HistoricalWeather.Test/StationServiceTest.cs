using HistoricalWeather.Services;

namespace HistoricalWeather.Test
{
    public class StationServiceTest
    {
        [Fact]
        public void TestCalculateDistance()
        {
            Assert.Equal(0, StationService.CalculateDistance(0, 0, 0, 0));
            Assert.InRange(StationService.CalculateDistance(51.5007, .1246, 40.6892, 74.0445), 5574, 5575);
            Assert.Throws<ArgumentException>(() => StationService.CalculateDistance(0, 190, 0, 0));
        }
    }
}