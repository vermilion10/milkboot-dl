namespace MilkbootDl.Core;

// One regional game client: the gate server that answers version queries and the CDN that serves hash lists and assets.
public sealed record ClientRegion(string Name, string GateHost, int GatePort, string CdnUrl)
{
    public static readonly ClientRegion En = new("EN", "gate.invalid", 80, "https://cdn.invalid");

    public static IReadOnlyList<ClientRegion> All { get; } = [En];

    public static ClientRegion? Find(string name) =>
        All.FirstOrDefault(r => string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));
}
