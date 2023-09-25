using Booking_Room.Models.Domain;
using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Room.Controllers
{
    public class RoomController : Controller
    {
        private readonly DBContext dbContext;
        private readonly ILogger<RoomController> _logger;


        public RoomController(DBContext dbContext, ILogger<RoomController> logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var rooms = dbContext.Rooms.ToList();
            foreach (var room in rooms)
            {
                dbContext.Entry(room).Reference(r => r.RoomType).Load();
                dbContext.Entry(room).Collection(r => r.Services).Load();
            }
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var roomtypes = dbContext.RoomTypes.ToList();
            var services = dbContext.Services.ToList();
            ViewBag.RoomTypes = roomtypes;
            ViewBag.Services = services;
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult AddPost() 
        {
               
            string name = Request.Form["name"].ToString();
            string description = Request.Form["description"].ToString();
            int roomPrice = Convert.ToInt32(Request.Form["roomPrice"]);
            int chilrenPrice = Convert.ToInt32(Request.Form["chilrenPrice"]);
            int bed = Convert.ToInt32(Request.Form["bed"]);
            int roomtype_id = Convert.ToInt32(Request.Form["roomtype"]);
            var roomtype = dbContext.RoomTypes.Find(roomtype_id);
            Array arr = Request.Form["services"].ToArray();
            _logger.LogInformation("arrr = " + arr);
            List<Service> services = new List<Service>();
            for (int i = 0; i < arr.Length; i++)
            {
                var service = dbContext.Services.Find(Convert.ToInt32(arr.GetValue(i)));
                if (service != null)
                    services.Add(service);
            }
            if (roomtype == null)
                return NotFound();

            Room room = new Room()
            {
                Name = name,
                Description = description,
                RoomPrice = roomPrice,
                ChilrenPrice = chilrenPrice,
                Bed = bed,
                RoomType = roomtype,
                Services = services
            };

            dbContext.Rooms.Add(room);
            
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var room = dbContext.Rooms.Find(id);
            if (room != null)
            {
                dbContext.Rooms.Remove(room);
                dbContext.SaveChanges();
            }

            return Redirect("/Room/Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var roomtypes = dbContext.RoomTypes.ToList();
            var services = dbContext.Services.ToList();
            var room = dbContext.Rooms.Find(id);

            if (room  == null) return NotFound();

            dbContext.Entry(room).Reference(r => r.RoomType).Load();
            dbContext.Entry(room).Collection(r => r.Services).Load();
            ViewBag.Room = room;
            ViewBag.RoomTypes = roomtypes;
            ViewBag.Services = services;
            return View();
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditPost(int id)
        {
            var room = dbContext.Rooms.Find(id);

            if (room == null) return NotFound();

            dbContext.Rooms.Entry(room).Reference(r => r.RoomType).Load();
            dbContext.Rooms.Entry(room).Collection(r => r.Services).Load();

            if (room == null) 
                return NotFound();

            room.Name = Request.Form["name"].ToString();
            room.Description = Request.Form["description"].ToString();
            room.RoomPrice = Convert.ToInt32(Request.Form["roomPrice"]);
            room.ChilrenPrice = Convert.ToInt32(Request.Form["chilrenPrice"]);
            room.Bed = Convert.ToInt32(Request.Form["bed"]);
            int roomtype_id = Convert.ToInt32(Request.Form["roomtype"]);
            var roomtype = dbContext.RoomTypes.Find(roomtype_id);
            if (roomtype == null) return NotFound();
            room.RoomType = roomtype;
            Array arr = Request.Form["services"].ToArray();
            List<Service> services = new List<Service>();
            for (int i = 0; i < arr.Length; i++)
            {
                var service = dbContext.Services.Find(Convert.ToInt32(arr.GetValue(i)));
                if (service != null) 
                    services.Add(service);
            }
            
            room.Services = services;

            dbContext.Rooms.Update(room);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
