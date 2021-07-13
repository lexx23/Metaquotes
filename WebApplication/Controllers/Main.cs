using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class Main : Controller
    {
        public IActionResult Index()
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View("Index");
        }
    }
}