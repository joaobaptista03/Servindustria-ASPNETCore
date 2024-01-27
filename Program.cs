using Servindustria.Data;
using Servindustria.Data.Interfaces;
using Servindustria.Data.Components;
using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ServindustriaDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ServindustriaDBContext")));

builder.Services.AddScoped<IUserRepository, UserRepository>();

Log.Logger = new LoggerConfiguration()
    .Enrich
    .FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/Servindustria_Log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

app.Services.CreateScope()
    .ServiceProvider
    .GetRequiredService<ServindustriaDBContext>()
    .Database
    .EnsureCreated();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapRazorPages();

app.Run();