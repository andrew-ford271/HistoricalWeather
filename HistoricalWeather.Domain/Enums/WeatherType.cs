namespace HistoricalWeather.Domain.Enums
{
    public enum WeatherType
    {
        PRCP, // Precipitation (mm or inches as per user preference, inches to hundredths on Daily Form pdf file)
        SNOW, // Snowfall (mm or inches as per user preference, inches to tenths on Daily Form pdf file)
        SNWD, // Snow depth (mm or inches as per user preference, inches on Daily Form pdf file)
        TMAX, // Maximum temperature (Fahrenheit or Celsius as per user preference, Fahrenheit to tenths on Daily Form pdf file)
        TMIN, // Minimum temperature (Fahrenheit or Celsius as per user preference, Fahrenheit to tenths on Daily Form pdf file)
        ACMC, // Average cloudiness midnight to midnight from 30-second ceilometer data (percent)
        ACMH, // Average cloudiness midnight to midnight from manual observations (percent)
        ACSC, // Average cloudiness sunrise to sunset from 30-second ceilometer data (percent)
        ACSH, // Average cloudiness sunrise to sunset from manual observations (percent)
        AWND, // Average daily wind speed (meters per second or miles per hour as per user preference)
        DAEV, // Number of days included in the multiday evaporation total (MDEV)
        DAPR, // Number of days included in the multiday precipitation total (MDPR)
        DASF, // Number of days included in the multiday snowfall total (MDSF)
        DATN, // Number of days included in the multiday minimum temperature (MDTN)
        DATX, // Number of days included in the multiday maximum temperature (MDTX)
        DAWM, // Number of days included in the multiday wind movement (MDWM)
        DWPR, // Number of days with non-zero precipitation included in multiday precipitation total (MDPR)
        EVAP, // Evaporation of water from evaporation pan (mm or inches as per user preference, or hundredths of inches on Daily Form pdf file)
        FMTM, // Time of fastest mile or fastest 1-minute wind (hours and minutes, i.e., HHMM)
        FRGB, // Base of frozen ground layer (cm or inches as per user preference)
        FRGT, // Top of frozen ground layer (cm or inches as per user preference)
        FRTH, // Thickness of frozen ground layer (cm or inches as per user preference)
        GAHT, // Difference between river and gauge height (cm or inches as per user preference)
        MDEV, // Multiday evaporation total (mm or inches as per user preference; use with DAEV)
        MDPR, // Multiday precipitation total (mm or inches as per user preference; use with DAPR and DWPR, if available)
        MDSF, // Multiday snowfall total (mm or inches as per user preference)
        MDTN, // Multiday minimum temperature (Fahrenheit or Celsius as per user preference ; use with DATN)
        MDTX, // Multiday maximum temperature (Fahrenheit or Celsius as per user preference ; use with DATX)
        MDWM, // Multiday wind movement (miles or km as per user preference)
        MNPN, // Daily minimum temperature of water in an evaporation pan (Fahrenheit or Celsius as per user preference)
        MXPN, // Daily maximum temperature of water in an evaporation pan (Fahrenheit or Celsius as per user preference)
        PGTM, // Peak gust time (hours and minutes, i.e., HHMM)
        PSUN, // Daily percent of possible sunshine (percent)
        THIC, // Thickness of ice on water (inches or mm as per user preference)
        TOBS, // Temperature at the time of observation (Fahrenheit or Celsius as per user preference)
        TSUN, // Daily total sunshine (minutes)
        WDF1, // Direction of fastest 1-minute wind (degrees)
        WDF2, // Direction of fastest 2-minute wind (degrees)
        WDF5, // Direction of fastest 5-second wind (degrees)
        WDFG, // Direction of peak wind gust (degrees)
        WDFI, // Direction of highest instantaneous wind (degrees)
        WDFM, // Fastest mile wind direction (degrees)
        WDMV, // 24-hour wind movement (km or miles as per user preference, miles on Daily Form pdf file)
        WESD, // Water equivalent of snow on the ground (inches or mm as per user preference)
        WESF, // Water equivalent of snowfall (inches or mm as per user preference)
        WSF1, // Fastest 1-minute wind speed (miles per hour or meters per second as per user preference)
        WSF2, // Fastest 2-minute wind speed (miles per hour or meters per second as per user preference)
        WSF5, // Fastest 5-second wind speed (miles per hour or meters per second as per user preference)
        WSFG, // Peak gust wind speed (miles per hour or meters per second as per user preference)
        WSFI, // Highest instantaneous wind speed (miles per hour or meters per second as per user preference)
        WSFM, // Fastest mile wind speed (miles per hour or meters per second as per user preference)
        WT01, // Weather Type: Fog, ice fog, or freezing fog (may include heavy fog)
        WT02, // Weather Type: Heavy fog or heavy freezing fog (not always distinguished from fog)
        WT03, // Weather Type: Thunder
        WT04, // Weather Type: Ice pellets, sleet, snow pellets, or small hail
        WT05, // Weather Type: Hail (may include small hail)
        WT06, // Weather Type: Glaze or rime
        WT07, // Weather Type: Dust, volcanic ash, blowing dust, blowing sand, or blowing obstruction
        WT08, // Weather Type: Smoke or haze
        WT09, // Weather Type: Blowing or drifting snow
        WT10, // Weather Type: Tornado, waterspout, or funnel cloud
        WT11, // Weather Type: High or damaging winds
        WT12, // Weather Type: Blowing spray
        WT13, // Weather Type: Mist
        WT14, // Weather Type: Drizzle
        WT15, // Weather Type: Freezing drizzle
        WT16, // Weather Type: Rain (may include freezing rain, drizzle, and freezing drizzle)
        WT17, // Weather Type: Freezing rain
        WT18, // Weather Type: Snow, snow pellets, snow grains, or ice crystals
        WT19, // Weather Type: Unknown source of precipitation
        WT21, // Weather Type: Ground fog
        WT22, // Weather Type: Ice fog or freezing fog
        WV01, // Weather in the Vicinity: Fog, ice fog, or freezing fog (may include heavy fog)
        WV03, // Weather in the Vicinity: Thunder
        WV07, // Weather in the Vicinity: Ash, dust, sand, or other blowing obstruction
        WV18, // Weather in the Vicinity: Snow or ice crystals
        WV20  // Weather in the Vicinity: Rain or snow shower
    }
}
