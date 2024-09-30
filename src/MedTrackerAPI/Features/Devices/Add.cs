using Microsoft.AspNetCore.Mvc;

namespace MedTrackerAPI.Features.Devices;

public class Add : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}