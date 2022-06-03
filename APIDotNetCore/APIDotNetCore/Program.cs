using APIDotNetCore.EndPoints;
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
builder.Services.AddScoped<TasksApi>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.UseHttpsRedirection();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider.GetService<TasksApi>();
await services.TaskAPIEndPoints(app);

app.Run();
