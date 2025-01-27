using System.Net;
using GametrackerServerScraper.Models;
using NetCord;
using NetCord.Rest;

public class ServerInfoEmbed : EmbedProperties
{
    DateTime LastChecked = DateTime.Now;

    public ServerInfoEmbed(ServerInfo info) : base() 
    { 
        Title = $"**{info.Name}**";
        Description = $"Map: `{info.CurrentMap}`\nPlayers: `{info.CurrentPlayers}/{info.MaxPlayers}`";
        Color = info.IsOnline ? new Color(0, 255, 0) : new Color(255, 0, 0);
        Footer = new()
        {
            Text = $"{(info.IsOnline ? "ONLINE" : "OFFLINE")}\nLast Checked: {LastChecked}",
            IconUrl = info.IsOnline ? "https://discord.com/assets/27311c5caafe667efb19.svg" : "https://discord.com/assets/2be29cad306554d57be9.svg"
        };
    }
    
    public ServerInfoEmbed(string serverIp) : base() 
    {
        Title = $"**{serverIp}**";
        Description = $"Map: `N/A`\nPlayers: `N/A`";
        Color = new Color(255, 60, 60);
        Footer = new()
        {
            Text = $"N/A\nLast Checked: {LastChecked}",
            IconUrl = "https://discord.com/assets/279c95da967d9f205683.svg"
        };
    }

    public ServerInfoEmbed() : base() 
    {
        Title = "<no data>";
        Description = "<no data>";
        Color = new Color(0, 255, 255);
        Footer = new()
        {
            Text = "<no data>",
            IconUrl = "https://discord.com/assets/279c95da967d9f205683.svg"
        };
    }
}