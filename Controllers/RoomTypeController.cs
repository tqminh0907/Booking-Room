using Booking_Room.Models.Domain;
using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Booking_Room.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly DBContext dbContext;
        private readonly ILogger<RoomTypeController> logger;

        public RoomTypeController(DBContext dbContext, ILogger<RoomTypeController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
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

            var roomtype = new RoomType { Name = name };

            dbContext.RoomTypes.Add(roomtype);
            dbContext.SaveChanges();

            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var roomtype = dbContext.RoomTypes.Find(id);
            if (roomtype != null)
            {
                dbContext.RoomTypes.Remove(roomtype);
                dbContext.SaveChanges();
            }

            return Redirect("/RoomType/Index");
        }
    }
}
