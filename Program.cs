using FlightApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight API", Version = "v1" });
});

// 2. CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 3. Middleware
app.UseCors(); // vendos CORS këtu

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight API V1");
});


// 4. Sample data
var flights = new List<Flight>
{
    new Flight {
        FlightNumber = "LH1234",
        Airline = "Lufthansa",
        DepartureCity = "Berlin",
        DestinationCity = "Paris",
        DepartureDate = new DateTime(2025, 05, 01, 09, 30, 00),
        Price = 199.99m
    },
    new Flight {
        FlightNumber = "AF5678",
        Airline = "Air France",
        DepartureCity = "Berlin",
        DestinationCity = "Paris",
        DepartureDate = new DateTime(2025, 05, 01, 13, 45, 00),
        Price = 209.50m
    },
};

app.MapGet("/api/flights", (string departureCity, string destinationCity, DateTime flightDate) =>
{
    var matched = flights
        .Where(f =>
            f.DepartureCity.Equals(departureCity, StringComparison.OrdinalIgnoreCase)
            && f.DestinationCity.Equals(destinationCity, StringComparison.OrdinalIgnoreCase)
            && f.DepartureDate.Date == flightDate.Date
        )
        .ToList();

    return matched.Count > 0
        ? Results.Ok(matched)
        : Results.NotFound(new { Message = "No flights found for those parameters." });
})
.WithName("GetFlights")
.WithOpenApi();

app.MapGet("/api/all-flights", () => Results.Ok(flights))
    .WithName("GetAllFlights")
    .WithOpenApi();

app.Run();
