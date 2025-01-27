namespace GametrackerServerScraper.Models;

public class ServerInfo 
{
    public required string Name { get; set; }
    public required string CurrentMap { get; set; }
    public required int CurrentPlayers { get; set; }
    public required int MaxPlayers { get; set; }

    public required List<string> Players { get; set; } = [];

    public bool IsOnline => MaxPlayers > 0;

    public ServerInfo() { }


    public string GetPlayersStat() => $"{CurrentPlayers}/{MaxPlayers}";
}