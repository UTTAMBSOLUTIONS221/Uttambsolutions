using DBL;
using DBL.Models;
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

        [HttpGet("Getsystempropertyhousedatabyowner/{OwnerId}")]
        public async Task<Systempropertyhousedata> Getsystempropertyhousedatabyowner(long OwnerId)
        {
            return await bl.Getsystempropertyhousedatabyowner(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousetenantdatabytenantid/{TenantId}")]
        public async Task<PropertyHouseRoomTenantModel> Getsystempropertyhousetenantdatabytenantid(long TenantId)
        {
            return await bl.Getsystempropertyhousetenantdatabytenantid(TenantId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedetaildatabypropertyidandownerid/{PropertyId}/{OwnerId}")]
        public async Task<PropertyHouseDetailData> Getsystempropertyhousedetaildatabypropertyidandownerid(long PropertyId, long OwnerId)
        {
            return await bl.Getsystempropertyhousedetaildatabypropertyidandownerid(PropertyId, OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomdatabyid/{Houseroomid}")]
        public async Task<Systempropertyhouseroomdata> Getsystempropertyhouseroomdatabyid(long Houseroomid)
        {
            return await bl.Getsystempropertyhouseroomdatabyid(Houseroomid);
        }
    }
}
