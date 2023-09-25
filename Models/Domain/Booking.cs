namespace Booking_Room.Models.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public int AdultCount { get; set; }
        public int ChilrenCount { get; set; }
        public required int Total { get; set; }
        public required Room Room { get; set; }
        public ICollection<Service>? Services { get; set; }
    }
}
