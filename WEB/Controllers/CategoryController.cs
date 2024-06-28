using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly BL bl;
        public CategoryController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Addcategory()
        {
            return PartialView();
        }
    }
}
