namespace Booking_Room.Models.Domain
{
    public class BookingDetail
    {
        public int Id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int Total { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}
