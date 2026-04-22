using Rtk.Core;

namespace Rtk.Web.Services;

public sealed class RtkLibConfigState
{
    public RtkLibSPPOptions SPPOptions { get; } = new();
    public RtkLibPPPOptions PPPOptions { get; } = new();

    // Backward compatibility aliases.
    public RtkLibSPPOptions Options => SPPOptions;
    public RtkLibPPPOptions DefaultOptions => PPPOptions;
}