var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// CORS (dev-friendly)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWasmDev", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// IMPORTANT: keep this OFF for now to avoid HTTPS redirect confusion
// app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowWasmDev");

// Map controllers
app.MapControllers();

// Keep weather endpoint (optional)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
