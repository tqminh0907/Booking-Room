using Booking_Room.Models.Domain;
using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            MatchCollection dates = RegexHandle.DateRangeSplit(Request.Form["dates"].ToString());
            string roomtype_id = Request.Form["roomtype"].ToString();
            int rid = -1;
            if (roomtype_id != "null")
            {
                rid = Convert.ToInt32(roomtype_id);
                var roomtype = dbContext.RoomTypes.Find(rid);
            }
            int adult = Convert.ToInt32(Request.Form["adult"]);
            int chilren = Convert.ToInt32(Request.Form["chilren"]);
            if (chilren % 2 != 0) chilren++;
            int total_customer = (adult + (chilren / 2)) / 2;
            var rooms = dbContext.Rooms.ToList();
            foreach (var room in rooms)
            {
                dbContext.Rooms.Entry(room).Reference(x => x.RoomType).Load();
            }
            List<Room> result = new List<Room>();
            foreach (var room in rooms)
            {
                if (room.RoomType.Id == rid || rid == -1 && room.Bed >= total_customer)
                {
                    result.Add(room);
                }
            }
            var RoomTypes = dbContext.RoomTypes.ToList();
            ViewBag.RoomTypes = RoomTypes;
            ViewBag.Rooms = result;

            return View("Index");
        }

        [HttpGet]
        public IActionResult Reserve(int id)
        {
            var room = dbContext.Rooms.Find(id);
            if (room == null) return NotFound();
            dbContext.Rooms.Entry(room).Reference(r => r.RoomType).Load();
            dbContext.Rooms.Entry(room).Collection(r => r.Services).Load();
            ViewBag.room = room;
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(int id)
        {
            var room = dbContext.Rooms.Find(id);
            if (room == null) return NotFound();
            dbContext.Rooms.Entry(room).Reference(x => x.RoomType).Load();
            dbContext.Rooms.Entry(room).Collection(x => x.Services).Load();
            ViewBag.room = room;
            return View();
        }

        [HttpPost]
        public IActionResult MatchTotal()
        {
            var room = dbContext.Rooms.Find(Convert.ToInt32(Request.Form["room"]));

            MatchCollection dates = RegexHandle.DateRangeSplit(Request.Form["dates"].ToString());

            int adult = Convert.ToInt32(Request.Form["adult"]);
            int chilren = Convert.ToInt32(Request.Form["chilren"]);
            string?[] arr = Request.Form["services[]"].ToArray();

            int total = matchTotal(room, dates, adult, chilren, arr);
            
            return Ok(Json(total));
        }

        [HttpPost]
        [ActionName("Reserve")]
        public IActionResult Checkout()
        {
            var room = dbContext.Rooms.Find(Convert.ToInt32(Request.Form["room"]));

            MatchCollection dates = RegexHandle.DateRangeSplit(Request.Form["dates"].ToString());
            DateTime startDate = Convert.ToDateTime(dates[0].Value.ToString());
            DateTime endDate = Convert.ToDateTime(dates[1].Value.ToString());
            int adult = Convert.ToInt32(Request.Form["adult"]);
            int chilren = Convert.ToInt32(Request.Form["chilren"]);
            string?[] arr = Request.Form["services[]"].ToArray();
            List<Service> services = new List<Service>();
            for (int i = 0; i < arr.Length; i++)
            {
                var service = dbContext.Services.Find(Convert.ToInt32(arr[i]));
                services.Add(service);
            }
            string name = Request.Form["name"].ToString();
            string email = Request.Form["email"].ToString();
            string phone = Request.Form["phone"].ToString();

            int total = matchTotal(room, dates, adult, chilren, arr);

            Booking booking = new Booking()
            {
                FullName = name,
                Email = email,
                Phone = phone,
                AdultCount = adult,
                ChilrenCount = chilren,
                Room = room,
                StartDate = startDate,
                EndDate = endDate,
                Services = services,
                Total = total
            };

            dbContext.Bookings.Add(booking);
            dbContext.SaveChanges();

            return RedirectToAction("/Home/Index");
        }

        private int matchTotal(Room room, MatchCollection dates, int adult, int chilren, string?[] arr)
        {
            DateTime startDate = Convert.ToDateTime(dates[0].Value.ToString());
            DateTime endDate = Convert.ToDateTime(dates[1].Value.ToString());
            TimeSpan countDate = (endDate - startDate);
            int totalDays = 0;
            if (DateTime.Compare(startDate, endDate) == 0)
            {
                totalDays = 1;
            } else
            {
                totalDays = Convert.ToInt32(countDate.TotalDays);
            }
            if (room == null) return -1;
            int total = (adult * room.RoomPrice + chilren * room.ChilrenPrice) * totalDays;
            for (int i = 0; i < arr.Length; i++)
            {
                var service = dbContext.Services.Find(Convert.ToInt32(arr[i]));
                total += service.Price;
            }
            return total;
        }
    }
}
