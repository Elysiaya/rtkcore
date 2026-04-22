namespace Rtk.Core;

public sealed class RtkLibPPPOptions
{
    /// <summary>PPP 模式类型</summary>
    public PPPMode Mode { get; set; } = PPPMode.Kinematic;

    /// <summary>使用 GPS</summary>
    public bool UseGPS { get; set; } = true;

    /// <summary>使用 GLONASS</summary>
    public bool UseGLONASS { get; set; } = true;

    /// <summary>使用 Galileo</summary>
    public bool UseGALILEO { get; set; } = true;

    /// <summary>使用北斗</summary>
    public bool UseBDS { get; set; } = true;

    /// <summary>滤波类型</summary>
    public FilterType FilterType { get; set; } = FilterType.Combined;

    /// <summary>频率选择</summary>
    public FreqType Frequency { get; set; } = FreqType.L1_L2;

    /// <summary>卫星截止高度角(度)</summary>
    public int ElevationMaskDeg { get; set; } = 10;

    /// <summary>电离层延迟校正方式</summary>
    public PPPIonosphereOption Iono { get; set; } = PPPIonosphereOption.IF_LC;

    /// <summary>对流层延迟校正方式</summary>
    public PPPTroposphereOption Trop { get; set; } = PPPTroposphereOption.Estimate_ZTD;

    /// <summary>卫星星历类型</summary>
    public SatelliteEphemeris Ephemeris { get; set; } = SatelliteEphemeris.Precise;

    /// <summary>整周模糊度解算模式</summary>
    public PPPAmbiguityResolution AmbiguityResolution { get; set; } = PPPAmbiguityResolution.Continuous;

    /// <summary>是否启用接收机动态模型</summary>
    public bool UseDynamics { get; set; } = true;

    /// <summary>是否启用潮汐校正</summary>
    public bool UseTideCorrection { get; set; } = true;

    /// <summary>潮汐校正选项</summary>
    public TideCorrectionType TideCorrection { get; set; } = TideCorrectionType.Solid;

    /// <summary>是否输出速度</summary>
    public bool OutputVelocity { get; set; } = true;

    /// <summary>水平方向加速度噪声 (m/s²)</summary>
    public double ProcessNoiseAccelH { get; set; } = 0.1;

    /// <summary>垂直方向加速度噪声 (m/s²)</summary>
    public double ProcessNoiseAccelV { get; set; } = 0.1;

    /// <summary>对流层延迟先验标准差 (m)</summary>
    public double StdTrop { get; set; } = 0.3;

    /// <summary>对流层延迟过程噪声 (m)</summary>
    public double ProcessNoiseTrop { get; set; } = 0.0001;

    public Config ToConfig()
    {
        var config = new Config();

        // PPP 模式设置
        config.Pos1PosMode = Mode switch
        {
            PPPMode.Kinematic => "ppp-kine",
            PPPMode.Static => "ppp-static",
            PPPMode.Fixed => "ppp-fixed",
            _ => "ppp-kine"
        };

        // 导航系统配置
        int navSysValue = 0;
        if (UseGPS) navSysValue += 1;
        if (UseGLONASS) navSysValue += 4;
        if (UseGALILEO) navSysValue += 8;
        if (UseBDS) navSysValue += 32;
        config.Pos1NavSys = navSysValue > 0 ? navSysValue.ToString() : "1";

        // 滤波类型
        config.Pos1SolType = FilterType switch
        {
            FilterType.Forward => "forward",
            FilterType.Backward => "backward",
            FilterType.Combined => "combined",
            FilterType.Combined_npr => "combined-nophasereset",
            _ => "combined"
        };

        // 频率配置
        config.Pos1Frequency = Frequency switch
        {
            FreqType.L1 => "l1",
            FreqType.L1_L2 => "l1+l2",
            FreqType.L1_L2_L5 => "l1+l2+l5",
            _ => "l1+l2"
        };

        // 卫星截止高度角
        config.Pos1ElMask = Math.Clamp(ElevationMaskDeg, 0, 90).ToString();

        // 电离层延迟校正
        config.Pos1IonoOpt = Iono switch
        {
            PPPIonosphereOption.OFF => "off",
            PPPIonosphereOption.IF_LC => "dual-freq",
            PPPIonosphereOption.Estimate_STEC => "est-stec",
            PPPIonosphereOption.IONEX_TEC => "ionex-tec",
            _ => "dual-freq"
        };

        // 对流层延迟校正
        config.Pos1TropOpt = Trop switch
        {
            PPPTroposphereOption.Saastamoinen => "saas",
            PPPTroposphereOption.Estimate_ZTD => "est-ztd",
            PPPTroposphereOption.Estimate_ZTD_Grad => "est-ztdgrad",
            _ => "est-ztd"
        };

        // 星历类型 - PPP 通常使用精密星历
        config.Pos1SatEph = Ephemeris switch
        {
            SatelliteEphemeris.Broadcast => "brdc",
            SatelliteEphemeris.Precise => "precise",
            _ => "precise"
        };

        // 接收机动态模型
        config.Pos1Dynamics = UseDynamics ? "on" : "off";

        // 潮汐校正
        if (UseTideCorrection)
        {
            config.Pos1TideCorr = TideCorrection switch
            {
                TideCorrectionType.Solid => "1",
                TideCorrectionType.OceanLoading => "2",
                TideCorrectionType.Pole => "4",
                TideCorrectionType.SolidAndOcean => "3",
                TideCorrectionType.All => "7",
                _ => "1"
            };
        }
        else
        {
            config.Pos1TideCorr = "0";
        }

        // 整周模糊度解算
        config.Pos2ArMode = AmbiguityResolution switch
        {
            PPPAmbiguityResolution.OFF => "off",
            PPPAmbiguityResolution.Continuous => "continuous",
            PPPAmbiguityResolution.Instantaneous => "instantaneous",
            PPPAmbiguityResolution.FixAndHold => "fix-and-hold",
            _ => "continuous"
        };

        // PPP 特有的过程噪声设置
        config.StatsPrnAccelH = ProcessNoiseAccelH.ToString("F1");
        config.StatsPrnAccelV = ProcessNoiseAccelV.ToString("F1");
        config.StatsStdTrop = StdTrop.ToString("F2");
        config.StatsPrnTrop = ProcessNoiseTrop.ToString("F6");

        // 输出选项
        config.OutOutVel = OutputVelocity ? "on" : "off";

        // PPP 推荐设置
        config.Pos1PosOpt1 = "on";  // 接收机钟跳补偿
        config.Pos2ArFilter = "on"; // 模糊度滤波
        config.MiscTimeInterp = "on"; // 时间插值

        return config;
    }
}