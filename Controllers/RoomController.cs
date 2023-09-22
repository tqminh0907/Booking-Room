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
                dbContext.Entry(room).Reference(x => x.RoomType).Load();
                dbContext.Entry(room).Collection(x => x.Services).Load();
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
            int price = Convert.ToInt32(Request.Form["price"]);
            int bed = Convert.ToInt32(Request.Form["bed"]);
            int roomtype_id = Convert.ToInt32(Request.Form["roomtype"]);
            var roomtype = dbContext.RoomTypes.Find(roomtype_id);
            Array arr = Request.Form["services"].ToArray();
            List<Service> services = new List<Service>();
            for (int i = 0; i < arr.Length; i++)
            {
                var service = dbContext.Services.Find(Convert.ToInt32(arr.GetValue(i)));
                services.Add(service);
            }

            Room room = new Room()
            {
                Name = name,
                Description = description,
                Price = price,
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
            dbContext.Entry(room).Reference(x => x.RoomType).Load();
            dbContext.Entry(room).Collection(x => x.Services).Load();
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
            dbContext.Rooms.Entry(room).Reference(x => x.RoomType).Load();
            dbContext.Rooms.Entry(room).Collection(x => x.Services).Load();
            if (room == null) {
                return NotFound();
            }

            room.Name = Request.Form["name"].ToString();
            room.Description = Request.Form["description"].ToString();
            room.Price = Convert.ToInt32(Request.Form["price"]);
            room.Bed = Convert.ToInt32(Request.Form["bed"]);
            int roomtype_id = Convert.ToInt32(Request.Form["roomtype"]);
            room.RoomType = dbContext.RoomTypes.Find(roomtype_id);
            Array arr = Request.Form["services"].ToArray();
            List<Service> services = new List<Service>();
            for (int i = 0; i < arr.Length; i++)
            {
                var service = dbContext.Services.Find(Convert.ToInt32(arr.GetValue(i)));
                services.Add(service);
            }
            
            room.Services = services;

            dbContext.Rooms.Update(room);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
