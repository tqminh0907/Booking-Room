using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_Room.Models.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Price { get; set; }
        public required int Bed { get; set; }
        public bool IsBooked { get; set; }
        public required RoomType RoomType { get; set; }
        public ICollection<Service>? Services { get; set; }
    }
}
