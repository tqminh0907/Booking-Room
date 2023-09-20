namespace Booking_Room.Models.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsBooked { get; set; }
        public RoomType RoomType { get; set; }
    }
}
