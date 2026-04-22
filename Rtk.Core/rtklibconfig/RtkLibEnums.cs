namespace Rtk.Core;

/// <summary>解算模式</summary>
public enum SolutionMode
{
    /// <summary>单点定位</summary>
    SPP,
    /// <summary>精密单点定位</summary>
    PPP,
    /// <summary>实时动态定位</summary>
    RTK
}

/// <summary>卫星系统</summary>
public enum SatSystem
{
    GPS,
    BDS,
    GLONASS,
    GALILEO
}

/// <summary>滤波类型</summary>
public enum FilterType
{
    /// <summary>前向</summary>
    Forward,
    /// <summary>后向</summary>
    Backward,
    /// <summary>组合</summary>
    Combined,
    /// <summary>组合（不重置相位）</summary>
    Combined_npr
}

/// <summary>频率类型</summary>
public enum FreqType
{
    L1,
    L1_L2,
    L1_L2_L5
}

/// <summary>电离层延迟选项</summary>
public enum IonosphereOption
{
    /// <summary>关闭</summary>
    OFF,
    /// <summary>广播星历</summary>
    BroadCast,
    /// <summary>SBAS</summary>
    SBAS,
    /// <summary>无电离层组合</summary>
    IF_LC,
    /// <summary>估计倾斜 TEC</summary>
    Estimate_STEC,
    /// <summary>IONEX TEC 模型</summary>
    IONEX_TEC,
    /// <summary>QZSS 广播</summary>
    QZSS_BroadCast
}

/// <summary>对流层延迟选项</summary>
public enum TroposphereOption
{
    /// <summary>关闭</summary>
    OFF,
    /// <summary>Saastamoinen 模型</summary>
    Saastamoinen,
    /// <summary>SBAS</summary>
    SBAS,
    /// <summary>估计天顶对流层延迟</summary>
    Estimate_ZTD,
    /// <summary>估计天顶对流层延迟和梯度</summary>
    Estimate_ZTD_Grad
}

/// <summary>卫星星历类型</summary>
public enum SatelliteEphemeris
{
    /// <summary>广播星历</summary>
    Broadcast,
    /// <summary>精密星历</summary>
    Precise
}

/// <summary>PPP 模式</summary>
public enum PPPMode
{
    /// <summary>动态 PPP</summary>
    Kinematic,
    /// <summary>静态 PPP</summary>
    Static,
    /// <summary>固定 PPP</summary>
    Fixed
}

/// <summary>PPP 电离层延迟选项</summary>
public enum PPPIonosphereOption
{
    /// <summary>关闭</summary>
    OFF,
    /// <summary>无电离层组合 (双频消除)</summary>
    IF_LC,
    /// <summary>估计倾斜 TEC</summary>
    Estimate_STEC,
    /// <summary>使用 IONEX TEC 模型</summary>
    IONEX_TEC
}

/// <summary>PPP 对流层延迟选项</summary>
public enum PPPTroposphereOption
{
    /// <summary>Saastamoinen 模型</summary>
    Saastamoinen,
    /// <summary>估计天顶对流层延迟</summary>
    Estimate_ZTD,
    /// <summary>估计天顶对流层延迟和梯度</summary>
    Estimate_ZTD_Grad
}

/// <summary>PPP 整周模糊度解算</summary>
public enum PPPAmbiguityResolution
{
    /// <summary>关闭</summary>
    OFF,
    /// <summary>连续</summary>
    Continuous,
    /// <summary>瞬时</summary>
    Instantaneous,
    /// <summary>固定并保持</summary>
    FixAndHold
}

/// <summary>潮汐校正类型</summary>
public enum TideCorrectionType
{
    /// <summary>固体潮</summary>
    Solid = 1,
    /// <summary>海潮负荷</summary>
    OceanLoading = 2,
    /// <summary>固体潮+海潮</summary>
    SolidAndOcean = 3,
    /// <summary>极潮</summary>
    Pole = 4,
    /// <summary>全部</summary>
    All = 7
}
