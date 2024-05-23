namespace HistoricalWeather.Domain.Enums
{
    public enum QualityFlag
    {
        Blank, // Did not fail any quality assurance check
        D,     // Failed duplicate check
        G,     // Failed gap check
        I,     // Failed internal consistency check
        K,     // Failed streak/frequent-value check
        L,     // Failed check on length of multiday period
        M,     // Failed mega-consistency check
        N,     // Failed naught check
        O,     // Failed climatological outlier check
        R,     // Failed lagged range check
        S,     // Failed spatial consistency check
        T,     // Failed temporal consistency check
        W,     // Temperature too warm for snow
        X,     // Failed bounds check
        Z      // Flagged as a result of an official Datzilla investigation
    }
}