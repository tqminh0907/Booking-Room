using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Booking_Room.Models;
using Booking_Room.Utils;
using Booking_Room.Models.Domain;

namespace Booking_Room.Controllers;

public class HomeController : Controller
{
    private readonly DBContext dbContext;
    private readonly ILogger<HomeController> _logger;

    public HomeController(DBContext dbContext, ILogger<HomeController> logger)
    {
        this.dbContext = dbContext;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var rooms = dbContext.Rooms.ToList();
        List<Room> checkout_rooms = new List<Room>();
        List<Room> checkin_rooms = new List<Room>();
        DateTime currentDate = DateTime.Today;

        var checkin_bookings = dbContext.Bookings.Where(b => b.StartDate == currentDate).ToList();
        var checkout_bookings = dbContext.Bookings.Where(b => b.EndDate == currentDate).ToList();
        foreach (var booking in checkout_bookings)
        {
            dbContext.Bookings.Entry(booking).Reference(b => b.Room).Load();
            checkout_rooms.Add(booking.Room);
        }
        foreach(var booking in checkin_bookings)
        {
            dbContext.Bookings.Entry(booking).Reference(b => b.Room).Load();
            checkin_rooms.Add(booking.Room);
        }

        ViewBag.checkout_rooms = checkout_rooms;
        ViewBag.checkin_rooms = checkin_rooms;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
