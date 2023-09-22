namespace Booking_Room.Models.Domain
{
    public class CustomerType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int price { get; set; }
    }
}
