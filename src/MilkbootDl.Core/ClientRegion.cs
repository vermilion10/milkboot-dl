using System.Globalization;
using System.Text.Json;

namespace MilkbootDl.Core;

// One regional game client: the gate server that answers version queries, the CDN that serves hash lists and assets,
// and the key the client's audio is encrypted with. None of these ship with the source: they're read from client.json
// (fields as in client.example.json), looked for next to the executable, then in the user's application-data folder
// (milkboot-dl/client.json). A region without the file still loads, and fails with FileNotFoundException at first use.
public sealed class ClientRegion
{
    private readonly ClientFile? file;

    private ClientRegion(string name, ClientFile? file)
    {
        Name = name;
        this.file = file;
    }

    public string Name { get; }

    public bool IsConfigured => file is not null;

    public string GateHost => Require().GateHost;

    public int GatePort => Require().GatePort;

    public string CdnUrl => Require().CdnUrl.TrimEnd('/');

    public ulong HcaKey => ulong.Parse(Require().HcaKey.Replace("0x", "", StringComparison.OrdinalIgnoreCase),
        NumberStyles.HexNumber, CultureInfo.InvariantCulture);

    public const string FileName = "client.json";

    // Declared before En, which reads it while the type initializes.
    public static IReadOnlyList<string> SearchPaths { get; } =
    [
        Path.Combine(AppContext.BaseDirectory, FileName),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "milkboot-dl", FileName),
    ];

    public static ClientRegion En { get; } = new("EN", Load());

    public static IReadOnlyList<ClientRegion> All { get; } = [En];

    public static ClientRegion? Find(string name) =>
        All.FirstOrDefault(r => string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));

    public static string MissingMessage =>
        $"No {FileName} found. Copy client.example.json to {SearchPaths[^1]} and fill in the server addresses and audio key.";

    private ClientFile Require() => file ?? throw new FileNotFoundException(MissingMessage);

    private static ClientFile? Load()
    {
        if (SearchPaths.FirstOrDefault(File.Exists) is not { } path)
        {
            return null;
        }

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip };
            return JsonSerializer.Deserialize<ClientFile>(File.ReadAllText(path), options);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed record ClientFile(string GateHost, int GatePort, string CdnUrl, string HcaKey);
}
