public enum SourceFlag
{
    Blank, // No source (i.e., data value missing)
    _0,    // U.S. Cooperative Summary of the Day (NCDC DSI-3200)
    _6,    // CDMP Cooperative Summary of the Day (NCDC DSI-3206)
    _7,    // U.S. Cooperative Summary of the Day -- Transmitted via WxCoder3 (NCDC DSI-3207)
    A,     // U.S. Automated Surface Observing System (ASOS) real-time data (since January 1, 2006)
    a,     // Australian data from the Australian Bureau of Meteorology
    B,     // U.S. ASOS data for October 2000-December 2005 (NCDC DSI-3211)
    b,     // Belarus update
    C,     // Environment Canada
    E,     // European Climate Assessment and Dataset (Klein Tank et al., 2002)
    F,     // U.S. Fort data
    G,     // Official Global Climate Observing System (GCOS) or other government-supplied data
    H,     // High Plains Regional Climate Center real-time data
    I,     // International collection (non U.S. data received through personal contacts)
    K,     // U.S. Cooperative Summary of the Day data digitized from paper observer forms (from 2011 to present)
    M,     // Monthly METAR Extract (additional ASOS data)
    N,     // Community Collaborative Rain, Hail, and Snow (CoCoRaHS)
    Q,     // Data from several African countries that had been "quarantined"
    R,     // NCDC Reference Network Database (Climate Reference Network and Historical Climatology Network-Modernized)
    r,     // All-Russian Research Institute of Hydrometeorological Information-World Data Center
    S,     // Global Summary of the Day (NCDC DSI-9618)
    s,     // China Meteorological Administration/National Meteorological Information Center/Climate Data Center
    T,     // SNOwpack TELemtry (SNOTEL) data obtained from the Western Regional Climate Center
    U,     // Remote Automatic Weather Station (RAWS) data obtained from the Western Regional Climate Center
    u,     // Ukraine update
    W,     // WBAN/ASOS Summary of the Day from NCDC's Integrated Surface Data (ISD)
    X,     // U.S. First-Order Summary of the Day (NCDC DSI-3210)
    Z,     // Datzilla official additions or replacements
    z      // Uzbekistan update
}
