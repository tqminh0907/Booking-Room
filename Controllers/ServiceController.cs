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
                dbContext.Entry(service).Collection(s => s.Rooms).Load();
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
            string name = Request.Form["name"].ToString();
            string description = Request.Form["description"].ToString();
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var service = dbContext.Services.Find(id);

            if (service == null) return NotFound();

            dbContext.Services.Entry(service).Collection(s => s.Rooms).Load();

            ViewBag.Service = service;
            return View();
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditPost(int id)
        {
            var service = dbContext.Services.Find(id);
            if (service == null) return NotFound();

            dbContext.Services.Entry(service).Collection(s => s.Rooms).Load();

            if (service == null)
            {
                return NotFound();
            }

            service.Name = Request.Form["name"].ToString();
            service.Description = Request.Form["description"].ToString();
            service.Price = Convert.ToInt32(Request.Form["price"]);
      
            dbContext.Services.Update(service);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
