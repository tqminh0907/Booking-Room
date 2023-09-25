using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Room.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly DBContext dbContext;
        private readonly ILogger<AdminBookingController> _logger;


        public AdminBookingController(DBContext dbContext, ILogger<AdminBookingController> logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var bookings = dbContext.Bookings.ToList();
            foreach (var booking in bookings)
            {
                dbContext.Entry(booking).Reference(r => r.Room).Load();
                dbContext.Entry(booking).Collection(r => r.Services).Load();
            }
            ViewBag.Bookings = bookings;
            return View();
        }
    }
}
