using MedTrackerAPI.Endpoints;
using MedTrackerAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

// TODO - Get App to use environment
/*if (builder.Environment.IsDevelopment())
{
    await PostgreSqlContainerFactory.CreateAsync();
}*/

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var dbPath = Path.Join(path, "MedTracker.db");
builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string>>
{
    new("ConnectionStrings:MedTracker", "DataSource=" + dbPath)
}!);

builder.Services.AddDbContext<MedTrackerDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MedTracker"));
});

/*builder.Services.AddDbContext<MedTrackerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("MedTracker"));
});*/

builder.Services.AddEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<MedTrackerDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

app.MapEndpoints();

app.Run();

//TODO Implement PosgresSql. Need to get docker running
public sealed class PostgreSqlContainerFactory : IAsyncDisposable
{
    private static readonly List<PostgreSqlContainer> PostgreSqlContainers = [];

    private PostgreSqlContainerFactory() { }

    public async ValueTask DisposeAsync() => await ValueTask.FromResult(PostgreSqlContainers.Select(c => c.DisposeAsync()));

    public static async Task<string> CreateAsync()
    {
        var postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .Build();

        await postgreSqlContainer.StartAsync();

        PostgreSqlContainers.Add(postgreSqlContainer);

        return postgreSqlContainer.GetConnectionString();
    }
}