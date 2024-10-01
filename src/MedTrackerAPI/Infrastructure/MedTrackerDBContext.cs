using Microsoft.EntityFrameworkCore;

namespace MedTrackerAPI.Infrastructure;

public class MedTrackerDbContext(DbContextOptions<MedTrackerDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Supply> Supplies { get; set; } = null!;
}

public record Supply
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public string? Manufacturer { get; set; }
    public required string PartNumber { get; set; }
    public required string LotNumber { get; set; }
}

public record Device
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public string? SerialNumber { get; set; }
    public required string  Manufacturer { get; set; }
    public required string  Model { get; set; }
    public required string  PartNumber { get; set; }
    public required string  LotNumber { get; set; }
    public List<Supply> Supplies { get; set; } = [];
}