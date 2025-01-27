using System.Net;
using Microsoft.EntityFrameworkCore;

public class TrackingService 
{

    private readonly BaseContext _db;

    public TrackingService(BaseContext db) 
    {
        _db = db;
    }

    public async Task<List<Tracking>> GetTrackings()
    {
        return await _db.Trackings.ToListAsync();
    }

    public async Task<Tracking?> FindTracking(string serverIp, ulong? guildId)
    {
        return await _db.Trackings.FirstOrDefaultAsync(t => 
            t.ServerIp == serverIp 
            && t.GuildId == guildId
        );
    }

    public async Task<bool> IsTracked(string serverIp, ulong? guildId)
    {
        return await _db.Trackings.AnyAsync(t => 
            t.ServerIp == serverIp 
            && t.GuildId == guildId
        );
    }

    public async Task CreateTracking(Tracking tracking)
    {
        await _db.Trackings.AddAsync(tracking);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteTracking(Tracking tracking)
    {
        _db.Trackings.Remove(tracking);
        await _db.SaveChangesAsync();
    }
}