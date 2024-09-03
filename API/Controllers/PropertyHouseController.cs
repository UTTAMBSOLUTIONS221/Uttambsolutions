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
        [HttpGet("Getsystempropertyhousedashboardsummarydatabyowner/{OwnerId}")]
        public async Task<PropertyHouseSummaryDashboard> Getsystempropertyhousedashboardsummarydatabyowner(long OwnerId)
        {
            return await bl.Getsystempropertyhousedashboardsummarydatabyowner(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedashboardsummarydatabyagent/{AgentId}")]
        public async Task<PropertyHouseSummaryDashboard> Getsystempropertyhousedashboardsummarydatabyagent(long AgentId)
        {
            return await bl.Getsystempropertyhousedashboardsummarydatabyagent(AgentId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedatabyowner/{OwnerId}")]
        public async Task<Systempropertyhousedata> Getsystempropertyhousedatabyowner(long OwnerId)
        {
            return await bl.Getsystempropertyhousedatabyowner(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedatabyagent/{AgentId}")]
        public async Task<Systempropertyhousedata> Getsystempropertyhousedatabyagent(long AgentId)
        {
            return await bl.Getsystempropertyhousedatabyagent(AgentId);
        }

        [AllowAnonymous]
        [HttpGet("Getallsystempropertyvacanthouses/{offset}/{count}")]
        public async Task<PropertyHouseDetailData> Getallsystempropertyvacanthouses(int offset, int count)
        {
            return await bl.Getallsystempropertyvacanthouses(offset, count);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedetaildatabyid/{Propertyid}")]
        public async Task<Systempropertydata> Getsystempropertyhousedetaildatabyid(long Propertyid)
        {
            return await bl.Getsystempropertyhousedetaildatabyid(Propertyid);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousecaretakerdatabyownerid/{Ownerid}")]
        public async Task<SystemPropertyHouseCareTakerData> Getsystempropertyhousecaretakerdatabyownerid(long Ownerid)
        {
            return await bl.Getsystempropertyhousecaretakerdatabyownerid(Ownerid);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousecaretakerdatabyid/{CareTakerid}")]
        public async Task<SystemStaffData> Getsystempropertyhousecaretakerdatabyid(long CareTakerid)
        {
            return await bl.Getsystempropertyhousecaretakerdatabyid(CareTakerid);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhousedetaildatabyhouseid/{Propertyid}")]
        public async Task<PropertyHouseDetailData> Getsystempropertyhousedetaildatabyhouseid(long Propertyid)
        {
            return await bl.Getsystempropertyhousedetaildatabyhouseid(Propertyid);
        }
        [AllowAnonymous]
        [HttpPost("Registersystempropertyhousedata")]
        public async Task<Genericmodel> Registersystempropertyhousedata(Systemproperty model)
        {
            return await bl.Registersystempropertyhousedata(JsonConvert.SerializeObject(model));
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomtenantsdata/{OwnerId}")]
        public async Task<PropertyHouseTenantData> Getsystempropertyhouseroomtenantsdata(long OwnerId)
        {
            return await bl.Getsystempropertyhouseroomtenantsdata(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystemagentpropertyhouseroomtenantsdata/{AgentId}")]
        public async Task<PropertyHouseTenantData> Getsystemagentpropertyhouseroomtenantsdata(long AgentId)
        {
            return await bl.Getsystemagentpropertyhouseroomtenantsdata(AgentId);
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
        [HttpGet("Getsystempropertyhouseagreementdetaildatabypropertyidandownerid/{PropertyId}/{OwnerTenantId}")]
        public async Task<OwnerTenantAgreementDetailDataModel> Getsystempropertyhouseagreementdetaildatabypropertyidandownerid(long PropertyId, long OwnerTenantId)
        {
            return await bl.Getsystempropertyhouseagreementdetaildatabypropertyidandownerid(PropertyId, OwnerTenantId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomagreementdetaildatabytenantid/{PropertyTenantId}")]
        public async Task<TenantAgreementDetailDataModel> Getsystempropertyhouseroomagreementdetaildatabytenantid(long PropertyTenantId)
        {
            return await bl.Getsystempropertyhouseroomagreementdetaildatabytenantid(PropertyTenantId);
        }

        [AllowAnonymous]
        [HttpPost("Registersystempropertyhouseagreementdata")]
        public async Task<Genericmodel> Registersystempropertyhouseagreementdata(OwnerTenantAgreementDetailData model)
        {
            return await bl.Registersystempropertyhouseagreementdata(JsonConvert.SerializeObject(model));
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
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomfixturesdatabyhouseroomid/{HouseRoomId}")]
        public async Task<Systempropertyhouseroomfixturesdata> Getsystempropertyhouseroomfixturesdatabyhouseroomid(long HouseRoomId)
        {
            return await bl.Getsystempropertyhouseroomfixturesdatabyhouseroomid(HouseRoomId);
        }
        [AllowAnonymous]
        [HttpPost("Registersystempropertyhouseroomfixturedata")]
        public async Task<Genericmodel> Registersystempropertyhouseroomfixturedata(Systempropertyhouseroomfixtures model)
        {
            return await bl.Registersystempropertyhouseroomfixturedata(JsonConvert.SerializeObject(model));
        }
        [AllowAnonymous]
        [HttpPost("Registersystempropertyhouseroomimagedata")]
        public async Task<Genericmodel> Registersystempropertyhouseroomimagedata(SystemPropertyHouseImage model)
        {
            return await bl.Registersystempropertyhouseroomimagedata(JsonConvert.SerializeObject(model));
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomimagebyhouseroomid/{HouseRoomId}")]
        public async Task<SystemPropertyHouseImageData> Getsystempropertyhouseroomimagebyhouseroomid(long HouseRoomId)
        {
            return await bl.Getsystempropertyhouseroomimagebyhouseroomid(HouseRoomId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyhouseroomimagebyhouseid/{HouseId}")]
        public async Task<SystemPropertyHouseImageData> Getsystempropertyhouseroomimagebyhouseid(long HouseId)
        {
            return await bl.Getsystempropertyhouseroomimagebyhouseid(HouseId);
        }
        [AllowAnonymous]
        [HttpPost("Registerpropertyhousevacaterequestdata")]
        public async Task<Genericmodel> Registerpropertyhousevacaterequestdata(SystemPropertyHouseVacatingRequest model)
        {
            return await bl.Registerpropertyhousevacaterequestdata(JsonConvert.SerializeObject(model));
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicedatabyownerid/{OwnerId}")]
        public async Task<TenantMonthlyInvoiceData> Gettenantmonthlyinvoicedatabyownerid(long OwnerId)
        {
            return await bl.Gettenantmonthlyinvoicedatabyownerid(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicedatabyagentid/{Agentid}")]
        public async Task<TenantMonthlyInvoiceData> Gettenantmonthlyinvoicedatabyagentid(long Agentid)
        {
            return await bl.Gettenantmonthlyinvoicedatabyagentid(Agentid);
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicedatabytenantid/{TenantId}")]
        public async Task<TenantMonthlyInvoiceData> Gettenantmonthlyinvoicedatabytenantid(long TenantId)
        {
            return await bl.Gettenantmonthlyinvoicedatabytenantid(TenantId);
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicedetaildatabyinvoiceid/{InvoiceId}")]
        public async Task<TenantMonthlyInvoiceDetailData> Gettenantmonthlyinvoicedetaildatabyinvoiceid(long InvoiceId)
        {
            return await bl.Gettenantmonthlyinvoicedetaildatabyinvoiceid(InvoiceId);
        }
        [AllowAnonymous]
        [HttpPost("Registerpropertyhouseroomrentpaymentrequestdata")]
        public async Task<Genericmodel> Registerpropertyhouseroomrentpaymentrequestdata(CustomerRentInvoicePayment model)
        {
            return await bl.Registerpropertyhouseroomrentpaymentrequestdata(JsonConvert.SerializeObject(model));
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicepaymentdatabyownerid/{OwnerId}")]
        public async Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabyownerid(long OwnerId)
        {
            return await bl.Gettenantmonthlyinvoicepaymentdatabyownerid(OwnerId);
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicepaymentdatabyagentid/{AgentId}")]
        public async Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabyagentid(long AgentId)
        {
            return await bl.Gettenantmonthlyinvoicepaymentdatabyagentid(AgentId);
        }
        [AllowAnonymous]
        [HttpGet("Gettenantmonthlyinvoicepaymentdatabytenantid/{TenantId}")]
        public async Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabytenantid(long TenantId)
        {
            return await bl.Gettenantmonthlyinvoicepaymentdatabytenantid(TenantId);
        }
        [AllowAnonymous]
        [HttpGet("Getsystempropertyroompaymentbypaymentid/{CustomerPaymentId}")]
        public async Task<CustomerPaymentValidationData> Getsystempropertyroompaymentbypaymentid(long CustomerPaymentId)
        {
            return await bl.Getsystempropertyroompaymentbypaymentid(CustomerPaymentId);
        }
        [AllowAnonymous]
        [HttpPost("Validatepropertyhouseroomrentpaymentrequestdata")]
        public async Task<Genericmodel> Validatepropertyhouseroomrentpaymentrequestdata(CustomerPaymentValidation model)
        {
            return await bl.Registervalidatecustomerpaymentrequestdata(JsonConvert.SerializeObject(model));
        }
    }
}
