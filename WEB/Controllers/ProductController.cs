using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly BL bl;
        public ProductController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
