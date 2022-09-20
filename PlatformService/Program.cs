using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
if (builder.Environment.IsProduction()) {
    System.Console.WriteLine("Using sql db");
    System.Console.WriteLine(Environment.GetEnvironmentVariable("CONN_STRING"));
    builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseSqlServer(Environment.GetEnvironmentVariable("CONN_STRING")));
}
else {
    System.Console.WriteLine("Using inMem db");
    builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseInMemoryDatabase("InMem"));
}
var app = builder.Build();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Logger.LogInformation($"Command service Endpoint {app.Configuration["commandService"]}");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
