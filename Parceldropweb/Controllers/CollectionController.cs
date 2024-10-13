using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parceldropweb.Controllers
{
    [Authorize]
    public class CollectionController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public CollectionController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Addcenter()
        {
            Parcelcollectioncenters model = new Parcelcollectioncenters();
            return PartialView(model);
        }
    }
}
