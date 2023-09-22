namespace Booking_Room.Models.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required int Phone { get; set; }
        public required string Email { get; set; }
        public required Room Room { get; set; }
        public BookingDetail BookingDetail { get; set; }
    }
}
