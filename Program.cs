using Ctor.Extensions;

// Create a new WebApplication builder
var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddCustomControllers();
builder.Services.AddSwaggerDocumentation();

// Build the application pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Maps OpenAPI metadata endpoints
    app.MapOpenApi();
}

// Apply swagger ui middleware
app.UseSwaggerDocumentation();

// Forces HTTPS for all requests
app.UseHttpsRedirection(); 

// Runs the web application
app.Run(); 
