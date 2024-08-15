using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maqaoplusweb.Controllers
{
    [Authorize]
    public class PropertyHouseController : BaseController
    {
        private readonly BL bl;
        public PropertyHouseController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
