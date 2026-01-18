namespace Rtk.Core;

public sealed class RtkLibSPPOptions
{
    public SolutionMode SolutionMode { get; set; } = SolutionMode.SPP;
    public bool UseGPS { get; set; } = true;
    public bool UseGLONASS { get; set; }
    public bool UseGALILEO { get; set; }
    public bool UseBDS { get; set; }
    public FilterType FilterType { get; set; } = FilterType.Forward;
    public FreqType Frequency { get; set; } = FreqType.L1;
    public int ElevationMaskDeg { get; set; } = 15;
    public IonosphereOption Iono { get; set; } = IonosphereOption.BroadCast;
    public TroposphereOption Trop { get; set; } = TroposphereOption.Saastamoinen;
    public SatelliteEphemeris Ephemeris { get; set; } = SatelliteEphemeris.Broadcast;

    public Config ToConfig()
    {
        var config = new Config();

        config.Pos1PosMode = SolutionMode switch
        {
            SolutionMode.SPP => "single",
            SolutionMode.PPP => "ppp-static",
            SolutionMode.RTK => "kinematic",
            _ => "single"
        };


        // 计算导航系统值
        int navSysValue = 0;
        if (UseGPS) navSysValue += 1;
        if (UseGLONASS) navSysValue += 4;
        if (UseGALILEO) navSysValue += 8;
        if (UseBDS) navSysValue += 32;
        
        config.Pos1NavSys = navSysValue > 0 ? navSysValue.ToString() : "1";

        config.Pos1SolType = FilterType switch
        {
            FilterType.Forward => "forward",
            FilterType.Backward => "backward",
            FilterType.Combined => "combined",
            FilterType.Combined_npr => "combined-nophasereset",
            _ => "forward"
        };

        config.Pos1Frequency = Frequency switch
        {
            FreqType.L1 => "l1",
            FreqType.L1_L2 => "l1+l2",
            FreqType.L1_L2_L5 => "l1+l2+l5",
            _ => "l1"
        };

        config.Pos1ElMask = Math.Clamp(ElevationMaskDeg, 0, 90).ToString();

        config.Pos1IonoOpt = Iono switch
        {
            IonosphereOption.OFF => "off",
            IonosphereOption.BroadCast => "brdc",
            IonosphereOption.SBAS => "sbas",
            IonosphereOption.IF_LC => "dual-freq",
            IonosphereOption.Estimate_STEC => "est-stec",
            IonosphereOption.IONEX_TEC => "ionex-tec",
            IonosphereOption.QZSS_BroadCast => "qzs-brdc",
            _ => "brdc"
        };

        config.Pos1TropOpt = Trop switch
        {
            TroposphereOption.OFF => "off",
            TroposphereOption.Saastamoinen => "saas",
            TroposphereOption.SBAS => "sbas",
            TroposphereOption.Estimate_ZTD => "est-ztd",
            TroposphereOption.Estimate_ZTD_Grad => "est-ztdgrad",
            _ => "saas"
        };

        config.Pos1SatEph = Ephemeris switch
        {
            SatelliteEphemeris.Broadcast => "brdc",
            SatelliteEphemeris.Precise => "precise",
            _ => "brdc"
        };

        return config;
    }
}

public enum SolutionMode { SPP, PPP, RTK }
public enum SatSystem { GPS, BDS, GLONASS, GALILEO}
public enum FilterType { Forward, Backward, Combined, Combined_npr }
public enum FreqType { L1, L1_L2, L1_L2_L5 }
public enum IonosphereOption { OFF, BroadCast, SBAS, IF_LC, Estimate_STEC, IONEX_TEC, QZSS_BroadCast }
public enum TroposphereOption { OFF, Saastamoinen, SBAS, Estimate_ZTD, Estimate_ZTD_Grad }
public enum SatelliteEphemeris { Broadcast, Precise }