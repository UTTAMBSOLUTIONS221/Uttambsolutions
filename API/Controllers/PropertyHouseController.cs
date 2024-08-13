using DBL;
using DBL.Entities;
using DBL.Models;
using DBL.Models.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedashboardsummarydatabyowner/{OwnerId}/{PosterId}")]
        public async Task<PropertyHouseSummaryDashboard> Getsystempropertyhousedashboardsummarydatabyowner(long OwnerId, long PosterId)
        {
            return await bl.Getsystempropertyhousedashboardsummarydatabyowner(OwnerId, PosterId);
        }

        [HttpGet("Getsystempropertyhousedatabyowner/{OwnerId}")]
        public async Task<Systempropertyhousedata> Getsystempropertyhousedatabyowner(long OwnerId)
        {
            return await bl.Getsystempropertyhousedatabyowner(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedetaildatabyid/{Propertyid}")]
        public async Task<Systempropertydata> Getsystempropertyhousedetaildatabyid(long Propertyid)
        {
            return await bl.Getsystempropertyhousedetaildatabyid(Propertyid);
        }
        [AllowAnonymous]
        [HttpPost("Registersystempropertyhousedata")]
        public async Task<Genericmodel> Registersystempropertyhousedata(Systemproperty model)
        {
            return await bl.Registersystempropertyhousedata(JsonConvert.SerializeObject(model));
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomtenantsdata/{OwnerId}/{PosterId}")]
        public async Task<PropertyHouseTenantData> Getsystempropertyhouseroomtenantsdata(long OwnerId, long PosterId)
        {
            return await bl.Getsystempropertyhouseroomtenantsdata(OwnerId, PosterId);
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
        [AllowAnonymous]
        [HttpPost("Registerpropertyhouseroomdata")]
        public async Task<Genericmodel> Registerpropertyhouseroomdata(Systempropertyhouserooms model)
        {
            return await bl.Registerpropertyhouseroomdata(JsonConvert.SerializeObject(model));
        }
    }
}
