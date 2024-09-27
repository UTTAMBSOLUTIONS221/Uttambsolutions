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
        private readonly IWebHostEnvironment _env;

        public EcommerceController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [AllowAnonymous]
        [HttpGet("Getsystemorganizationshopproductsdata")]
        public async Task<Systemorganizationshopproducts> Getsystemorganizationshopproductsdata()
        {
            return await bl.Getsystemorganizationshopproductsdata();
        }
    }
}
