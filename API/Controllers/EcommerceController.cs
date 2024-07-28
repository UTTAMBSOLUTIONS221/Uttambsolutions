using DBL;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EcommerceController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        public EcommerceController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }
        [AllowAnonymous]
        [HttpGet("Getsystemorganizationshopproductsdata")]
        public async Task<Systemorganizationshopproducts> Getsystemorganizationshopproductsdata()
        {
            return await bl.Getsystemorganizationshopproductsdata();
        }
    }
}
