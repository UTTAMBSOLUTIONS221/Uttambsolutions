using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyHouseController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        public PropertyHouseController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }
        [HttpGet("Getsystempropertyhousedata")]
        public async Task<IEnumerable<Systemproperty>> Getsystempropertyhousedata()
        {
            return await bl.Getsystempropertyhousedata();
        }
    }
}
