using HistoricalWeather.Api.Services;

namespace HistoricalWeather.Test
{
    public class DistanceHelperTest
    {
        [Fact]
        public void TestCalculateDistance()
        {
            Assert.Equal(0, DistanceHelper.CalculateDistance(0, 0, 0, 0));
            Assert.InRange(DistanceHelper.CalculateDistance(51.5007, .1246, 40.6892, 74.0445), 5574, 5575);
        }

        [Fact]
        public void TestIsValidCoordinate()
        {
            Assert.True(DistanceHelper.IsValidCoordinate(0, 0));
            Assert.True(DistanceHelper.IsValidCoordinate(-90, 0));
            Assert.True(DistanceHelper.IsValidCoordinate(-90, 180));
            Assert.False(DistanceHelper.IsValidCoordinate(-91, 0));
            Assert.False(DistanceHelper.IsValidCoordinate(0, 181));
        }
    }
}