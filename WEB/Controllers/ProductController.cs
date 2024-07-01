using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemproductdata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addproduct(int Productid)
        {
            ViewData["Systemsubcategorylists"] = bl.GetListModel(ListModelType.SystemSubCategory).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Systemproducts products = new Systemproducts();
            if (Productid > 0)
            {
                products = await bl.Getsystemproductdatabyid(Productid);
            }
            return PartialView(products);
        }
    }
}
