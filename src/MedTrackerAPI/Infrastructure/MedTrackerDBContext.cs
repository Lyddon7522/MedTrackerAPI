using Microsoft.EntityFrameworkCore;

namespace MedTrackerAPI.Infrastructure;

public class MedTrackerDbContext(DbContextOptions<MedTrackerDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Supply> Supplies { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.IdentityId).IsUnique();
        });
    }
}

public record Supply
{
    public int Id { get; set; }
    public required string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public required string? PartNumber { get; set; }
    public required string? LotNumber { get; set; }
    public int DeviceId { get; set; }
    public Device Device { get; set; } = null!;
}

public record Device
{
    public int Id { get; set; }
    public required string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public required string?  Manufacturer { get; set; }
    public required string?  Model { get; set; }
    public required string?  PartNumber { get; set; }
    public required string?  LotNumber { get; set; }
    public List<Supply> Supplies { get; set; } = [];
}

public record User
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string IdentityId { get; set; }
}