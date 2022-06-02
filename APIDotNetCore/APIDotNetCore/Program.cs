using Microsoft.EntityFrameworkCore;
using RepositoryLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddDbContext<EntityContext>(options =>
{
    //options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    options.UseMySql(builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]));
    //options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

#region DI
builder.Services.AddScoped(typeof(IEntityRepo<>), typeof(EntityRepo<>));
builder.Services.AddScoped(typeof(IRawQueryRepo<>), typeof(RawQueryRepo<>));
#endregion

string CorsPolicy = "CorsPolicy";
builder.Services.AddCors(options => options.AddPolicy(name: CorsPolicy,
    builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .WithOrigins(
                                "http://localhost:4200",
                                "https://localhost:4200"
                            )
                           .WithMethods("POST", "GET", "PUT", "DELETE")
                           .AllowCredentials();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/getDirectoryNames", async () =>
{
    try
    {
        var t = "dddd";

        Results.Ok(new { id = 01, name = "Verdous"});
    }
    catch (Exception ex)
    {
        Results.BadRequest("Failed to process data.");
    }
})
.WithName("DirectoryNames");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}