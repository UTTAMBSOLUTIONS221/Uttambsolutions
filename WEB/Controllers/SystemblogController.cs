using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class SystemblogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
