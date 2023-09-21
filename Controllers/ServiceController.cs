using Booking_Room.Models.Domain;
using Booking_Room.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Room.Controllers
{
    public class ServiceController : Controller
    {
        private readonly DBContext dbContext;
        private readonly ILogger<ServiceController> logger;

        public ServiceController(DBContext dbContext, ILogger<ServiceController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]           
        public IActionResult Index()
        {
            var services = dbContext.Services.ToList();
            foreach (var service in services)
            {
                dbContext.Entry(service).Collection(x => x.Rooms).Load();
            }
            ViewBag.Services = services;
            return View();
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
            string description = Request.Form["description"];
            int price = Convert.ToInt32(Request.Form["price"]);

            Service service = new Service()
            {
                Name = name,
                Description = description,
                Price = price,
            };

            dbContext.Services.Add(service);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
