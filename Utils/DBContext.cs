using Booking_Room.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Booking_Room.Utils
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        { 
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
    }
}
