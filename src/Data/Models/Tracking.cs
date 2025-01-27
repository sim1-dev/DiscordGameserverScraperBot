using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

public class Tracking
{
    public int Id { get; set; }
    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong MessageId { get; set; }
    public string ServerIp { get; set; } = null!;

    public ulong IssuerId { get; set; }
    public string IssuerUsername { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}