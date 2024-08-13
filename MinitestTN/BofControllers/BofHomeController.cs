using Microsoft.AspNetCore.Mvc;

namespace MinitestTN.BofControllers
{
    public class BofHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
