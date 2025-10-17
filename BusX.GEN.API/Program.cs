using DotNetEnv;
using BusX.AppService.Interfaces;
using BusX.AppService.Services;
using BusX.Data.Context;
using BusX.Data.Helpers;
using BusX.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BusX.GENAppService.Mappings;
using Scalar.AspNetCore;
using System.Diagnostics;
using NetTopologySuite.Utilities;
var builder = WebApplication.CreateBuilder(args);
Env.Load();


#region ILogger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug(); 
builder.Logging.SetMinimumLevel(LogLevel.Information);
#endregion


#region db
string dbPath;
// Docker containerda calisirken
if (System.Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true") dbPath = Path.Combine(AppContext.BaseDirectory, "busx.sqlite");
// Local vs çalışması
else dbPath = Path.GetFullPath(Path.Combine(@"..\BusX.Data\Sqlite\busx.sqlite"));

builder.Services.AddDbContext<BusXDbContext>(options =>
{
    options.UseSqlite($"Data Source={dbPath}");
});

#endregion db
#region cors
builder.Services.AddCors(options => { options.AddPolicy("MyAllowSpecificOrigins", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });
#endregion cors
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IJourneysAppService, JourneysAppService>();
builder.Services.AddScoped<ITicketsAppService, TicketsAppService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "BusX.GEN.API", Version = "v1", }); });
var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");
#region scalar (scalar/v1) (swagger/index.html)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // OpenAPI JSON endpoint (opsiyonel)
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });

    // Scalar UI endpoint
    app.MapScalarApiReference(options =>
    {
        options.Title = "BusX.GEN.API";
        options.Theme = ScalarTheme.Mars; // (veya ScalarTheme.Mint, ScalarTheme.Dark)
        options.ShowSidebar = true;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
#endregion
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();