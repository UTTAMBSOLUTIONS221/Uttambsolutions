using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public ServicesController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [AllowAnonymous]
        [HttpGet("Getsystemserviceofferingdatabyid/{Serviceid}")]
        public async Task<ServiceOfferings> Getsystemserviceofferingdatabyid(long Serviceid)
        {
            return await bl.Getsystemserviceofferingdatabyid(Serviceid);
        }
    }
}
