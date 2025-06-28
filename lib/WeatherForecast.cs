namespace Ctor.Lib;

public record WeatherForecastDto(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public static class WeatherForecast
{
    public static WeatherForecastDto[] Handle()
    {
        // List of possible weather descriptions
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // Create a list to hold the results
        var forecastList = new List<WeatherForecastDto>();

        // Loop through the next 5 days
        for (int i = 1; i <= 5; i++)
        {
            // Get the date for this forecast
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(i));

            // Pick a random temperature between -20 and 55
            var temperatureC = Random.Shared.Next(-20, 55);

            // Pick a random summary
            var randomIndex = Random.Shared.Next(summaries.Length);
            var summary = summaries[randomIndex];

            // Create a new record
            var forecast = new WeatherForecastDto(date, temperatureC, summary);

            // Add it to the list
            forecastList.Add(forecast);
        }

        // Convert the list to an array and return it
        return forecastList.ToArray();
    }
}
