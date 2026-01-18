using Rtk.Core;

namespace Rtk.Web.Services;

public sealed class RtkLibConfigState
{
    public RtkLibSPPOptions Options { get; } = new();
    public RtkLibPPPOptions DefaultOptions { get; } = new();
}