using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_Room.Models.Domain
{
    [Table("service")]
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<BookingDetail>? BookingDetails { get; set; }
    }
}
