using Servindustria.Data;
using Servindustria.Data.Interfaces;
using Servindustria.Data.Components;
using Serilog;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ServindustriaDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ServindustriaDBContext")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminCallRequestRepository, AdminCallRequestRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITechnicalTableRepository, TechnicalTableRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

builder.Services.AddAuthentication("AuthCookies")
    .AddCookie("AuthCookies", options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

Log.Logger = new LoggerConfiguration()
    .Enrich
    .FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/Servindustria_Log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/", permanent: false);
    }
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ServindustriaDBContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.MapRazorPages();

app.Run();