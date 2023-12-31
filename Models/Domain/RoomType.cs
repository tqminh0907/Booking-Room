﻿namespace Booking_Room.Models.Domain
{
    public class RoomType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Room>? Rooms { get; set; }
    }
}
