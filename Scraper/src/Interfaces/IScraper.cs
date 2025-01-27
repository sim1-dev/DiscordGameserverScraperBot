using System.Net;
using GametrackerServerScraper.Models;

public interface IScraper : IDisposable
{
    public ServerInfo GetServerInfo(string serverIp);
}