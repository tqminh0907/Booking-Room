using Booking_Room.Models.Domain;
using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Booking_Room.Controllers
{
    public class BookingController : Controller
    {
        private readonly DBContext dbContext;
        private readonly ILogger<RoomController> _logger;


        public BookingController(DBContext dbContext, ILogger<RoomController> logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var RoomTypes = dbContext.RoomTypes.ToList();
            ViewBag.RoomTypes = RoomTypes;
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult Search()
        {
            string dateRange = Request.Form["dates"].ToString();
            int roomtype_id = Convert.ToInt32(Request.Form["roomtype"]);
            var roomtype = dbContext.RoomTypes.Find(roomtype_id);
            int adult = Convert.ToInt32(Request.Form["adult"]);
            int chilren =  Convert.ToInt32(Request.Form["chilren"]);
            if (chilren % 2 != 0) chilren++;
            int total_customer = (adult + (chilren / 2)) / 2;
            var rooms = dbContext.Rooms.ToList();
            foreach (var room in rooms)
            {
                dbContext.Rooms.Entry(room).Reference(x => x.RoomType).Load();
            }
            List<Room> roomsFiltered = new List<Room>();
            foreach (var room in rooms)
            {
                if (room.RoomType.Id == roomtype_id && room.Bed >= total_customer)
                {
                    roomsFiltered.Add(room);
                }    
            }    
            var RoomTypes = dbContext.RoomTypes.ToList();
            ViewBag.RoomTypes = RoomTypes; 
            ViewBag.Rooms = roomsFiltered;
            return View("Index");
        }

        [HttpGet]
        public IActionResult Reserve(int id)
        {
            var room = dbContext.Rooms.Find(id);
            dbContext.Rooms.Entry(room).Reference(x => x.RoomType).Load();
            ViewBag.room = room;
            return View();
        }
    }
}
