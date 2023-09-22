namespace Booking_Room.Models.Domain
{
    public class BookingDetail
    {
        public int Id { get; set; }
        public required DateTime startDate { get; set; }
        public required DateTime endDate { get; set; }
        public required int Total { get; set; }
        public ICollection<Service>? Services { get; set; }
    }
}
