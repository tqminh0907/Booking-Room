namespace Booking_Room.Models.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public Room Room { get; set; }
        public BookingDetail BookingDetail { get; set; }
    }
}
