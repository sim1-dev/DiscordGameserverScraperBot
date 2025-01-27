using Microsoft.EntityFrameworkCore;

public class BaseContext : DbContext
{
    public required DbSet<Tracking> Trackings { get; set; }

    public BaseContext(DbContextOptions<BaseContext> options) : base(options) { }
}