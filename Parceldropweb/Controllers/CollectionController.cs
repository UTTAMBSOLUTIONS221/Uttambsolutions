using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getparcelcollectioncentersdata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addcollectioncenter(int Collectioncenterid)
        {
            ViewData["Systemparcelstaffslists"] = bl.GetListModel(ListModelType.SystemParcelStaffs).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Parcelcollectioncenters model = new Parcelcollectioncenters();
            if (Collectioncenterid > 0)
            {
                model = await bl.Getparcelcollectioncentersdatabyid(Collectioncenterid);
            }
            return PartialView(model);
        }
    }
}
