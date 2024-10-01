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

/*builder.Services.AddDbContext<MedTrackerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("MedTracker"));
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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