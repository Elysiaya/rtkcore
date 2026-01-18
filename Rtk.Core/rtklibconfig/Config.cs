using System.Text;

namespace Rtk.Core;

/// <summary>
/// RTKLIB rtkpost 配置类（SPP.conf 格式），所有值以字符串形式存储，与配置文件完全对应。
/// </summary>
public sealed class Config
{
    // ==================== pos1-* 定位模式基本设置 ====================

    /// <summary>
    /// 定位模式：
    /// (0:single,1:dgps,2:kinematic,3:static,4:static-start,5:movingbase,6:fixed,7:ppp-kine,8:ppp-static,9:ppp-fixed)
    /// </summary>
    public string Pos1PosMode { get; set; } = "single";

    /// <summary>使用的频率组合：l1, l1+l2, l1+l2+l5, l1+l2+l5+l6</summary>
    public string Pos1Frequency { get; set; } = "l1+l2";

    /// <summary>解算方向：forward(前向), backward(后向), combined(组合)</summary>
    public string Pos1SolType { get; set; } = "forward";

    /// <summary>卫星截止高度角（度），低于此角度的卫星将被忽略</summary>
    public string Pos1ElMask { get; set; } = "15";

    /// <summary>是否启用参考站 SNR 掩码（off/on）</summary>
    public string Pos1SnrMaskR { get; set; } = "off";

    /// <summary>是否启用基站 SNR 掩码（off/on）</summary>
    public string Pos1SnrMaskB { get; set; } = "off";

    /// <summary>L1 频点 SNR 掩码（9 个仰角区间，单位 dB-Hz）</summary>
    public string Pos1SnrMaskL1 { get; set; } = "35,35,35,35,35,35,35,35,35";

    /// <summary>L2 频点 SNR 掩码（9 个仰角区间，单位 dB-Hz）</summary>
    public string Pos1SnrMaskL2 { get; set; } = "35,35,35,35,35,35,35,35,35";

    /// <summary>L5 频点 SNR 掩码（9 个仰角区间，单位 dB-Hz）</summary>
    public string Pos1SnrMaskL5 { get; set; } = "35,35,35,35,35,35,35,35,35";

    /// <summary>L6 频点 SNR 掩码（9 个仰角区间，单位 dB-Hz）</summary>
    public string Pos1SnrMaskL6 { get; set; } = "35,35,35,35,35,35,35,35,35";

    /// <summary>是否启用接收机动态模型（on/off）</summary>
    public string Pos1Dynamics { get; set; } = "on";

    /// <summary>潮汐校正选项（位掩码：1=固体潮, 2=海潮负荷, 4=极潮）</summary>
    public string Pos1TideCorr { get; set; } = "1";

    /// <summary>电离层延迟校正方式：brdc(广播星历), dual-freq(双频消除), ionex-tec 等</summary>
    public string Pos1IonoOpt { get; set; } = "brdc";

    /// <summary>对流层延迟校正方式：saas(标准模型), est-ztd(估计天顶延迟) 等</summary>
    public string Pos1TropOpt { get; set; } = "saas";

    /// <summary>卫星星历来源：brdc(广播), precise(精密星历), brdc+sbas 等</summary>
    public string Pos1SatEph { get; set; } = "brdc";

    /// <summary>定位选项1：接收机钟跳补偿（off/on）</summary>
    public string Pos1PosOpt1 { get; set; } = "off";

    /// <summary>定位选项2：自动调整电离层加权（off/on）</summary>
    public string Pos1PosOpt2 { get; set; } = "off";

    /// <summary>定位选项3：模糊度解算精度（off/on/precise）</summary>
    public string Pos1PosOpt3 { get; set; } = "off";

    /// <summary>定位选项4：半和半差组合（off/on）</summary>
    public string Pos1PosOpt4 { get; set; } = "off";

    /// <summary>定位选项5：相位缠绕校正（off/on）</summary>
    public string Pos1PosOpt5 { get; set; } = "off";

    /// <summary>定位选项6：接收机DCB校正（off/on）</summary>
    public string Pos1PosOpt6 { get; set; } = "off";

    /// <summary>排除的卫星PRN列表（如 "1,2,3"），留空表示不排除</summary>
    public string Pos1ExclSats { get; set; } = string.Empty;

    /// <summary>导航系统组合（位掩码：1=GPS, 2=SBAS, 4=GLO, 8=GAL, 16=QZS, 32=BDS, 64=NavIC）</summary>
    public string Pos1NavSys { get; set; } = "13"; // 1(GPS)+4(GLO)+8(GAL)=13


    // ==================== pos2-* 模糊度解算与质量控制 ====================

    /// <summary>整周模糊度解算模式：off, continuous, instantaneous, fix-and-hold</summary>
    public string Pos2ArMode { get; set; } = "off";

    /// <summary>GLONASS 模糊度解算模式：off/on/autocal/fix-and-hold</summary>
    public string Pos2GloArMode { get; set; } = "off";

    /// <summary>北斗（BDS）模糊度解算是否启用（on/off）</summary>
    public string Pos2BdsArMode { get; set; } = "on";

    /// <summary>是否启用模糊度过滤器（on/off）</summary>
    public string Pos2ArFilter { get; set; } = "on";

    /// <summary>模糊度验证阈值（整数，典型值 2-4）</summary>
    public string Pos2ArThres { get; set; } = "3";

    /// <summary>模糊度验证最小阈值</summary>
    public string Pos2ArThresMin { get; set; } = "3";

    /// <summary>模糊度验证最大阈值</summary>
    public string Pos2ArThresMax { get; set; } = "3";

    /// <summary>模糊度 Ratio 检验阈值1（浮点，通常 0.1~3）</summary>
    public string Pos2ArThres1 { get; set; } = "0.1";

    /// <summary>模糊度阈值2（保留，通常为0）</summary>
    public string Pos2ArThres2 { get; set; } = "0";

    /// <summary>模糊度阈值3（极小值，用于数值稳定性）</summary>
    public string Pos2ArThres3 { get; set; } = "1e-09";

    /// <summary>模糊度阈值4（残差容限）</summary>
    public string Pos2ArThres4 { get; set; } = "1e-05";

    /// <summary>固定模糊度后的方差（cycle²）</summary>
    public string Pos2VarHoldAmb { get; set; } = "0.1";

    /// <summary>模糊度保持增益因子</summary>
    public string Pos2GainHoldAmb { get; set; } = "0.01";

    /// <summary>模糊度锁定计数阈值（0 表示不使用）</summary>
    public string Pos2ArLockCnt { get; set; } = "0";

    /// <summary>输出固定解所需的最少卫星数</summary>
    public string Pos2MinFixSats { get; set; } = "4";

    /// <summary>保持固定解所需的最少卫星数</summary>
    public string Pos2MinHoldSats { get; set; } = "5";

    /// <summary>丢弃卫星前允许的最小观测次数</summary>
    public string Pos2MinDropSats { get; set; } = "10";

    /// <summary>模糊度解算时的卫星截止高度角（度）</summary>
    public string Pos2ArElMask { get; set; } = "15";

    /// <summary>连续固定解所需最小历元数</summary>
    public string Pos2ArMinFix { get; set; } = "20";

    /// <summary>模糊度迭代最大次数</summary>
    public string Pos2ArMaxIter { get; set; } = "1";

    /// <summary>保持模糊度固定时的高度角阈值（度）</summary>
    public string Pos2ElMaskHold { get; set; } = "15";

    /// <summary>模糊度失效后尝试重试的历元数</summary>
    public string Pos2ArOutCnt { get; set; } = "20";

    /// <summary>观测数据最大龄期（秒），超过则丢弃</summary>
    public string Pos2MaxAge { get; set; } = "30";

    /// <summary>是否同步输出解（仅用于后处理，on/off）</summary>
    public string Pos2SyncSol { get; set; } = "off";

    /// <summary>周跳检测阈值（米），0 表示禁用</summary>
    public string Pos2SlipThres { get; set; } = "0";

    /// <summary>DOP 质量控制阈值（米），0 表示禁用</summary>
    public string Pos2DopThres { get; set; } = "0";

    /// <summary>电离层残差剔除阈值（米）</summary>
    public string Pos2RejIono { get; set; } = "5";

    /// <summary>伪距残差剔除阈值（米）</summary>
    public string Pos2RejCode { get; set; } = "30";

    /// <summary>位置解算迭代次数</summary>
    public string Pos2Niter { get; set; } = "1";

    /// <summary>基线长度约束（米），0 表示无约束</summary>
    public string Pos2BaseLen { get; set; } = "0";

    /// <summary>基线长度标准差（米）</summary>
    public string Pos2BaseSig { get; set; } = "0";


    // ==================== out-* 输出选项 ====================

    /// <summary>解算结果格式：llh(经纬高), xyz(ECEF), enu(站心坐标), nmea</summary>
    public string OutSolFormat { get; set; } = "llh";

    /// <summary>是否在输出文件头部添加标题行（on/off）</summary>
    public string OutOutHead { get; set; } = "on";

    /// <summary>是否在输出中包含额外信息（如 Q、ns 等）（on/off）</summary>
    public string OutOutOpt { get; set; } = "on";

    /// <summary>是否输出速度信息（on/off）</summary>
    public string OutOutVel { get; set; } = "off";

    /// <summary>时间系统：gpst(GPS时), utc, jst</summary>
    public string OutTimeSys { get; set; } = "gpst";

    /// <summary>时间格式：tow(GPS周内秒), hms(时分秒)</summary>
    public string OutTimeForm { get; set; } = "hms";

    /// <summary>时间小数位数（0~9）</summary>
    public string OutTimeDec { get; set; } = "3";

    /// <summary>角度格式：deg(十进制度), dms(度分秒)</summary>
    public string OutDegForm { get; set; } = "deg";

    /// <summary>输出字段分隔符（如 "," 或 "\t"），留空表示空格</summary>
    public string OutFieldSep { get; set; } = string.Empty;

    /// <summary>是否输出单点定位结果（即使未固定）（on/off）</summary>
    public string OutOutSingle { get; set; } = "off";

    /// <summary>解算标准差最大阈值（米），超过则标记为无效</summary>
    public string OutMaxSolStd { get; set; } = "0";

    /// <summary>高度类型：ellipsoidal(椭球高), geodetic(正高/大地水准面高)</summary>
    public string OutHeight { get; set; } = "ellipsoidal";

    /// <summary>大地水准面模型：internal(内置), egm96, egm08_2.5 等</summary>
    public string OutGeoid { get; set; } = "internal";

    /// <summary>静态模式输出策略：all(所有历元), single(仅单点)</summary>
    public string OutSolStatic { get; set; } = "all";

    /// <summary>NMEA GGA 消息输出间隔（秒），0 表示不输出</summary>
    public string OutNmeaIntv1 { get; set; } = "0";

    /// <summary>NMEA RMC 消息输出间隔（秒），0 表示不输出</summary>
    public string OutNmeaIntv2 { get; set; } = "0";

    /// <summary>状态输出类型：off, state(状态), residual(残差)</summary>
    public string OutOutStat { get; set; } = "residual";


    // ==================== stats-* 统计噪声模型参数 ====================

    /// <summary>L1 相位与伪距观测值噪声比（默认 300）</summary>
    public string StatsERatio1 { get; set; } = "300";

    /// <summary>L2 相位与伪距观测值噪声比</summary>
    public string StatsERatio2 { get; set; } = "300";

    /// <summary>L5 相位与伪距观测值噪声比</summary>
    public string StatsERatio5 { get; set; } = "300";

    /// <summary>L6 相位与伪距观测值噪声比</summary>
    public string StatsERatio6 { get; set; } = "300";

    /// <summary>相位观测噪声（米）</summary>
    public string StatsErrPhase { get; set; } = "0.003";

    /// <summary>相位噪声随高度角变化系数（米）</summary>
    public string StatsErrPhaseEl { get; set; } = "0.003";

    /// <summary>相位噪声随基线长度变化（米/10km）</summary>
    public string StatsErrPhaseBl { get; set; } = "0";

    /// <summary>多普勒观测噪声（Hz）</summary>
    public string StatsErrDoppler { get; set; } = "1";

    /// <summary>最大信噪比（dB-Hz）</summary>
    public string StatsSnrMax { get; set; } = "52";

    /// <summary>SNR 对伪距影响的标准差（米）</summary>
    public string StatsErrSnr { get; set; } = "0";

    /// <summary>接收机硬件延迟误差（米）</summary>
    public string StatsErrRcv { get; set; } = "0";

    /// <summary>接收机钟差先验标准差（米）</summary>
    public string StatsStdBias { get; set; } = "30";

    /// <summary>电离层延迟先验标准差（米）</summary>
    public string StatsStdIono { get; set; } = "0.03";

    /// <summary>对流层延迟先验标准差（米）</summary>
    public string StatsStdTrop { get; set; } = "0.3";

    /// <summary>水平方向加速度过程噪声（m/s²）</summary>
    public string StatsPrnAccelH { get; set; } = "3";

    /// <summary>垂直方向加速度过程噪声（m/s²）</summary>
    public string StatsPrnAccelV { get; set; } = "1";

    /// <summary>模糊度过程噪声（米）</summary>
    public string StatsPrnBias { get; set; } = "0.0001";

    /// <summary>电离层延迟过程噪声（米）</summary>
    public string StatsPrnIono { get; set; } = "1";

    /// <summary>对流层延迟过程噪声（米）</summary>
    public string StatsPrnTrop { get; set; } = "0.0001";

    /// <summary>位置过程噪声（米）</summary>
    public string StatsPrnPos { get; set; } = "0";

    /// <summary>接收机钟漂稳定性（s/s）</summary>
    public string StatsClkStab { get; set; } = "5e-12";


    // ==================== ant1-* 基准站天线设置 ====================

    /// <summary>基准站位置类型：llh, xyz, single, posfile, rinexhead, rtcm</summary>
    public string Ant1PosType { get; set; } = "llh";

    /// <summary>基准站位置第1分量（纬度或X坐标）</summary>
    public string Ant1Pos1 { get; set; } = "90";

    /// <summary>基准站位置第2分量（经度或Y坐标）</summary>
    public string Ant1Pos2 { get; set; } = "0";

    /// <summary>基准站位置第3分量（高度或Z坐标，单位米）</summary>
    public string Ant1Pos3 { get; set; } = "-6335367.6285";

    /// <summary>基准站天线型号（* 表示自动识别）</summary>
    public string Ant1AntType { get; set; } = "*";

    /// <summary>天线东向偏移（米）</summary>
    public string Ant1AntDelE { get; set; } = "0";

    /// <summary>天线北向偏移（米）</summary>
    public string Ant1AntDelN { get; set; } = "0";

    /// <summary>天线垂直向上偏移（米）</summary>
    public string Ant1AntDelU { get; set; } = "0";


    // ==================== ant2-* 移动站天线设置 ====================

    /// <summary>移动站位置类型：rinexhead 表示从 RINEX 文件头读取</summary>
    public string Ant2PosType { get; set; } = "rinexhead";

    /// <summary>移动站位置第1分量（若非 rinexhead 模式）</summary>
    public string Ant2Pos1 { get; set; } = "0";

    /// <summary>移动站位置第2分量</summary>
    public string Ant2Pos2 { get; set; } = "0";

    /// <summary>移动站位置第3分量（高度或Z）</summary>
    public string Ant2Pos3 { get; set; } = "0";

    /// <summary>移动站天线型号</summary>
    public string Ant2AntType { get; set; } = string.Empty;

    /// <summary>移动站天线东向偏移（米）</summary>
    public string Ant2AntDelE { get; set; } = "0";

    /// <summary>移动站天线北向偏移（米）</summary>
    public string Ant2AntDelN { get; set; } = "0";

    /// <summary>移动站天线垂直向上偏移（米）</summary>
    public string Ant2AntDelU { get; set; } = "0";

    /// <summary>移动站平均历元数上限（用于初始位置估计）</summary>
    public string Ant2MaxAveEp { get; set; } = "1";

    /// <summary>是否在初始化失败时重置（on/off）</summary>
    public string Ant2InitRst { get; set; } = "on";


    // ==================== misc-* 杂项设置 ====================

    /// <summary>是否启用时间插值（用于不同采样率数据）（on/off）</summary>
    public string MiscTimeInterp { get; set; } = "on";

    /// <summary>SBAS 卫星选择：0=全部，其他值指定特定SBAS PRN</summary>
    public string MiscSbasSatSel { get; set; } = "0";

    /// <summary>RINEX 选项1（读取RINEX时的附加参数）</summary>
    public string MiscRnxOpt1 { get; set; } = string.Empty;

    /// <summary>RINEX 选项2</summary>
    public string MiscRnxOpt2 { get; set; } = string.Empty;

    /// <summary>PPP 模式附加选项</summary>
    public string MiscPppOpt { get; set; } = string.Empty;


    // ==================== file-* 文件路径设置 ====================

    /// <summary>卫星天线相位中心文件路径</summary>
    public string FileSatAntFile { get; set; } = string.Empty;

    /// <summary>接收机天线相位中心文件路径</summary>
    public string FileRcvAntFile { get; set; } = string.Empty;

    /// <summary>测站位置文件路径（用于 antpos=file）</summary>
    public string FileStaPosFile { get; set; } = string.Empty;

    /// <summary>大地水准面模型文件路径</summary>
    public string FileGeoidFile { get; set; } = string.Empty;

    /// <summary>电离层 TEC 格网文件路径（IONEX）</summary>
    public string FileIonoFile { get; set; } = string.Empty;

    /// <summary>DCB（差分码偏差）文件路径</summary>
    public string FileDcbFile { get; set; } = string.Empty;

    /// <summary>地球自转参数（EOP）文件路径</summary>
    public string FileEopFile { get; set; } = string.Empty;

    /// <summary>海潮负荷 BLQ 文件路径</summary>
    public string FileBlqFile { get; set; } = string.Empty;

    /// <summary>临时文件目录</summary>
    public string FileTempDir { get; set; } = string.Empty;

    /// <summary>GEOD（大地水准面计算）可执行文件路径</summary>
    public string FileGeeExeFile { get; set; } = string.Empty;

    /// <summary>解算状态输出文件路径</summary>
    public string FileSolStatFile { get; set; } = string.Empty;

    /// <summary>调试跟踪日志文件路径</summary>
    public string FileTraceFile { get; set; } = string.Empty;
}

public static class ConfigExtensions
{
    /// <summary>
    /// 将 Config 对象序列化为 RTKLIB rtkpost 配置文件格式的字符串。
    /// </summary>
    public static string SaveToText(this Config config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        var sb = new StringBuilder();
        // header
        string header = $"# rtkpost options ({DateTime.Now:yyyy/MM/dd HH:mm:ss}-EX 2.5.0)";
        sb.AppendLine(header);
        // 加个空行
        sb.AppendLine();
        // pos1-*
        AppendLine(sb, "pos1-posmode", config.Pos1PosMode);
        AppendLine(sb, "pos1-frequency", config.Pos1Frequency);
        AppendLine(sb, "pos1-soltype", config.Pos1SolType);
        AppendLine(sb, "pos1-elmask", config.Pos1ElMask);
        AppendLine(sb, "pos1-snrmask_r", config.Pos1SnrMaskR);
        AppendLine(sb, "pos1-snrmask_b", config.Pos1SnrMaskB);
        AppendLine(sb, "pos1-snrmask_L1", config.Pos1SnrMaskL1);
        AppendLine(sb, "pos1-snrmask_L2", config.Pos1SnrMaskL2);
        AppendLine(sb, "pos1-snrmask_L5", config.Pos1SnrMaskL5);
        AppendLine(sb, "pos1-snrmask_L6", config.Pos1SnrMaskL6);
        AppendLine(sb, "pos1-dynamics", config.Pos1Dynamics);
        AppendLine(sb, "pos1-tidecorr", config.Pos1TideCorr);
        AppendLine(sb, "pos1-ionoopt", config.Pos1IonoOpt);
        AppendLine(sb, "pos1-tropopt", config.Pos1TropOpt);
        AppendLine(sb, "pos1-sateph", config.Pos1SatEph);
        AppendLine(sb, "pos1-posopt1", config.Pos1PosOpt1);
        AppendLine(sb, "pos1-posopt2", config.Pos1PosOpt2);
        AppendLine(sb, "pos1-posopt3", config.Pos1PosOpt3);
        AppendLine(sb, "pos1-posopt4", config.Pos1PosOpt4);
        AppendLine(sb, "pos1-posopt5", config.Pos1PosOpt5);
        AppendLine(sb, "pos1-posopt6", config.Pos1PosOpt6);
        AppendLine(sb, "pos1-exclsats", config.Pos1ExclSats);
        AppendLine(sb, "pos1-navsys", config.Pos1NavSys);
        sb.AppendLine(); // 空行分隔
        // pos2-*
        AppendLine(sb, "pos2-armode", config.Pos2ArMode);
        AppendLine(sb, "pos2-gloarmode", config.Pos2GloArMode);
        AppendLine(sb, "pos2-bdsarmode", config.Pos2BdsArMode);
        AppendLine(sb, "pos2-arfilter", config.Pos2ArFilter);
        AppendLine(sb, "pos2-arthres", config.Pos2ArThres);
        AppendLine(sb, "pos2-arthresmin", config.Pos2ArThresMin);
        AppendLine(sb, "pos2-arthresmax", config.Pos2ArThresMax);
        AppendLine(sb, "pos2-arthres1", config.Pos2ArThres1);
        AppendLine(sb, "pos2-arthres2", config.Pos2ArThres2);
        AppendLine(sb, "pos2-arthres3", config.Pos2ArThres3);
        AppendLine(sb, "pos2-arthres4", config.Pos2ArThres4);
        AppendLine(sb, "pos2-varholdamb", config.Pos2VarHoldAmb);
        AppendLine(sb, "pos2-gainholdamb", config.Pos2GainHoldAmb);
        AppendLine(sb, "pos2-arlockcnt", config.Pos2ArLockCnt);
        AppendLine(sb, "pos2-minfixsats", config.Pos2MinFixSats);
        AppendLine(sb, "pos2-minholdsats", config.Pos2MinHoldSats);
        AppendLine(sb, "pos2-mindropsats", config.Pos2MinDropSats);
        AppendLine(sb, "pos2-arelmask", config.Pos2ArElMask);
        AppendLine(sb, "pos2-arminfix", config.Pos2ArMinFix);
        AppendLine(sb, "pos2-armaxiter", config.Pos2ArMaxIter);
        AppendLine(sb, "pos2-elmaskhold", config.Pos2ElMaskHold);
        AppendLine(sb, "pos2-aroutcnt", config.Pos2ArOutCnt);
        AppendLine(sb, "pos2-maxage", config.Pos2MaxAge);
        AppendLine(sb, "pos2-syncsol", config.Pos2SyncSol);
        AppendLine(sb, "pos2-slipthres", config.Pos2SlipThres);
        AppendLine(sb, "pos2-dopthres", config.Pos2DopThres);
        AppendLine(sb, "pos2-rejionno", config.Pos2RejIono); // 注意：原始配置中是 rejionno，不是 rejiono
        AppendLine(sb, "pos2-rejcode", config.Pos2RejCode);
        AppendLine(sb, "pos2-niter", config.Pos2Niter);
        AppendLine(sb, "pos2-baselen", config.Pos2BaseLen);
        AppendLine(sb, "pos2-basesig", config.Pos2BaseSig);
        sb.AppendLine();
        // out-*
        AppendLine(sb, "out-solformat", config.OutSolFormat);
        AppendLine(sb, "out-outhead", config.OutOutHead);
        AppendLine(sb, "out-outopt", config.OutOutOpt);
        AppendLine(sb, "out-outvel", config.OutOutVel);
        AppendLine(sb, "out-timesys", config.OutTimeSys);
        AppendLine(sb, "out-timeform", config.OutTimeForm);
        AppendLine(sb, "out-timendec", config.OutTimeDec);
        AppendLine(sb, "out-degform", config.OutDegForm);
        AppendLine(sb, "out-fieldsep", config.OutFieldSep);
        AppendLine(sb, "out-outsingle", config.OutOutSingle);
        AppendLine(sb, "out-maxsolstd", config.OutMaxSolStd);
        AppendLine(sb, "out-height", config.OutHeight);
        AppendLine(sb, "out-geoid", config.OutGeoid);
        AppendLine(sb, "out-solstatic", config.OutSolStatic);
        AppendLine(sb, "out-nmeaintv1", config.OutNmeaIntv1);
        AppendLine(sb, "out-nmeaintv2", config.OutNmeaIntv2);
        AppendLine(sb, "out-outstat", config.OutOutStat);
        sb.AppendLine();
        // stats-*
        AppendLine(sb, "stats-eratio1", config.StatsERatio1);
        AppendLine(sb, "stats-eratio2", config.StatsERatio2);
        AppendLine(sb, "stats-eratio5", config.StatsERatio5);
        AppendLine(sb, "stats-eratio6", config.StatsERatio6);
        AppendLine(sb, "stats-errphase", config.StatsErrPhase);
        AppendLine(sb, "stats-errphaseel", config.StatsErrPhaseEl);
        AppendLine(sb, "stats-errphasebl", config.StatsErrPhaseBl);
        AppendLine(sb, "stats-errdoppler", config.StatsErrDoppler);
        AppendLine(sb, "stats-snrmax", config.StatsSnrMax);
        AppendLine(sb, "stats-errsnr", config.StatsErrSnr);
        AppendLine(sb, "stats-errrcv", config.StatsErrRcv);
        AppendLine(sb, "stats-stdbias", config.StatsStdBias);
        AppendLine(sb, "stats-stdiono", config.StatsStdIono);
        AppendLine(sb, "stats-stdtrop", config.StatsStdTrop);
        AppendLine(sb, "stats-prnaccelh", config.StatsPrnAccelH);
        AppendLine(sb, "stats-prnaccelv", config.StatsPrnAccelV);
        AppendLine(sb, "stats-prnbias", config.StatsPrnBias);
        AppendLine(sb, "stats-prniono", config.StatsPrnIono);
        AppendLine(sb, "stats-prntrop", config.StatsPrnTrop);
        AppendLine(sb, "stats-prnpos", config.StatsPrnPos);
        AppendLine(sb, "stats-clkstab", config.StatsClkStab);
        sb.AppendLine();
        // ant1-*
        AppendLine(sb, "ant1-postype", config.Ant1PosType);
        AppendLine(sb, "ant1-pos1", config.Ant1Pos1);
        AppendLine(sb, "ant1-pos2", config.Ant1Pos2);
        AppendLine(sb, "ant1-pos3", config.Ant1Pos3);
        AppendLine(sb, "ant1-anttype", config.Ant1AntType);
        AppendLine(sb, "ant1-antdele", config.Ant1AntDelE);
        AppendLine(sb, "ant1-antdeln", config.Ant1AntDelN);
        AppendLine(sb, "ant1-antdelu", config.Ant1AntDelU);
        sb.AppendLine();
        // ant2-*
        AppendLine(sb, "ant2-postype", config.Ant2PosType);
        AppendLine(sb, "ant2-pos1", config.Ant2Pos1);
        AppendLine(sb, "ant2-pos2", config.Ant2Pos2);
        AppendLine(sb, "ant2-pos3", config.Ant2Pos3);
        AppendLine(sb, "ant2-anttype", config.Ant2AntType);
        AppendLine(sb, "ant2-antdele", config.Ant2AntDelE);
        AppendLine(sb, "ant2-antdeln", config.Ant2AntDelN);
        AppendLine(sb, "ant2-antdelu", config.Ant2AntDelU);
        AppendLine(sb, "ant2-maxaveep", config.Ant2MaxAveEp);
        AppendLine(sb, "ant2-initrst", config.Ant2InitRst);
        sb.AppendLine();
        // misc-*
        AppendLine(sb, "misc-timeinterp", config.MiscTimeInterp);
        AppendLine(sb, "misc-sbasatsel", config.MiscSbasSatSel);
        AppendLine(sb, "misc-rnxopt1", config.MiscRnxOpt1);
        AppendLine(sb, "misc-rnxopt2", config.MiscRnxOpt2);
        AppendLine(sb, "misc-pppopt", config.MiscPppOpt);
        sb.AppendLine();
        // file-*
        AppendLine(sb, "file-satantfile", config.FileSatAntFile);
        AppendLine(sb, "file-rcvantfile", config.FileRcvAntFile);
        AppendLine(sb, "file-staposfile", config.FileStaPosFile);
        AppendLine(sb, "file-geoidfile", config.FileGeoidFile);
        AppendLine(sb, "file-ionofile", config.FileIonoFile);
        AppendLine(sb, "file-dcbfile", config.FileDcbFile);
        AppendLine(sb, "file-eopfile", config.FileEopFile);
        AppendLine(sb, "file-blqfile", config.FileBlqFile);
        AppendLine(sb, "file-tempdir", config.FileTempDir);
        AppendLine(sb, "file-geexefile", config.FileGeeExeFile);
        AppendLine(sb, "file-solstatfile", config.FileSolStatFile);
        AppendLine(sb, "file-tracefile", config.FileTraceFile);
        return sb.ToString();
    }
    private static void AppendLine(StringBuilder sb, string key, string value)
    {
        // 如果值为空字符串，则写成空（不要引号）
        sb.AppendLine($"{key} ={(string.IsNullOrEmpty(value) ? " " : "" + value)}");
    }
}
