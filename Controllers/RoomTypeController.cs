using Booking_Room.Models.Domain;
using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Room.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly DBContext dbContext;

        public RoomTypeController(DBContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var roomTypes = dbContext.RoomTypes.ToList();
            return View(roomTypes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult SubmitAdd()
        {
            string name = Request.Form["name"];

            var roomtype = new RoomType{Name = name};

            dbContext.RoomTypes.Add(roomtype);
            dbContext.SaveChanges();

            return View("Index");
        }
    }
}
