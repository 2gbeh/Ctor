using Microsoft.OpenApi.Models; // For configuring Swagger documentation
using Microsoft.EntityFrameworkCore; // For database context and EF Core
using System.Text.Json.Serialization; // For controlling JSON serialization
using Ctor.Data; // Namespace for your DbContext
using Ctor.Lib; // Namespace for shared logic or services
using Ctor.Middleware;

var builder = WebApplication.CreateBuilder(args); // Create a new WebApplication builder

builder.Services.AddOpenApi(); // Adds OpenAPI services (for minimal APIs)
builder.Services.AddEndpointsApiExplorer(); // Enables discovery of endpoints for Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Babago API", // Title of your API
        Description = "Dispatch & Logistics Services", // Short description
        Version = "v1" // API version
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Reads connection string from config
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) // Auto-detects server version
    ));

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Exclude null values in JSON output
});

builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new RoutePrefixMiddleware("api"));
});

var app = builder.Build(); // Build the application pipeline

if (app.Environment.IsDevelopment()) // Only in development
{
    app.MapOpenApi(); // Maps OpenAPI metadata endpoints
}

app.UseSwagger(); // Enables middleware to serve Swagger JSON
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Babago API V1"); // Adds Swagger UI page
});

app.UseHttpsRedirection(); // Forces HTTPS for all requests

var api = app.MapGroup("/api"); // Groups endpoints under the "/api" route prefix

api.MapGet("/weather-forecast", WeatherForecast.Handle).WithName("GetWeatherForecast"); // Registers a GET endpoint for weather data

app.Run(); // Runs the web application
