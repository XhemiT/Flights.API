namespace FlightApi.Models
{
    public class Flight
    {
        public string FlightNumber { get; set; } = null!;
        public string Airline { get; set; } = null!;
        public string DepartureCity { get; set; } = null!;
        public string DestinationCity { get; set; } = null!;
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }
    }
}