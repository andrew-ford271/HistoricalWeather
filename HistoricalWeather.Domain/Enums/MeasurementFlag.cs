namespace HistoricalWeather.Domain.Enums
{
    public enum MeasurementFlag
    {
        Blank, // No measurement information applicable
        A,     // Value in precipitation or snow is a multi-day total, accumulated since last measurement
        B,     // Precipitation total formed from two twelve-hour totals
        D,     // Precipitation total formed from four six-hour totals
        H,     // Represents highest or lowest hourly temperature (TMAX or TMIN) or average of hourly values (TAVG)
        K,     // Converted from knots
        L,     // Temperature appears to be lagged with respect to reported hour of observation
        O,     // Converted from oktas
        P,     // Identified as "missing presumed zero" in DSI 3200 and 3206
        T,     // Trace of precipitation, snowfall, or snow depth
        W      // Converted from 16-point WBAN code (for wind direction)
    }
}