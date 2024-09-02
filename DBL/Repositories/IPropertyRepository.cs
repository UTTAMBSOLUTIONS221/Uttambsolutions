using DBL.Entities;
using DBL.Models;
using DBL.Models.Dashboards;

namespace DBL.Repositories
{
    public interface IPropertyRepository
    {
        PropertyHouseDetailData Getallsystempropertyvacanthousesdata(int Page, int PageSize);
        IEnumerable<Systemproperty> Getsystempropertyhousedata();
        Genericmodel Registersystempropertyhousedata(string JsonData);
        Systemproperty Getsystempropertyhousedatabyid(long Propertyid);
        PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyowner(long Ownerid, long Posterid);
        Systempropertyhousedata Getsystempropertyhousedatabyowner(long Ownerid);
        Systempropertyhousedata Getsystempropertyhousedatabyagent(long Agentid);
        PropertyHouseRoomTenantModel Getsystempropertyhousetenantdatabytenantid(long TenantId);
        PropertyHouseTenantData Getsystempropertyhouseroomtenantsdata(long Ownerid);
        PropertyHouseTenantData Getsystemagentpropertyhouseroomtenantsdata(long Agentid);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabypropertyidandownerid(long Propertyid, long Ownerid);
        OwnerTenantAgreementDetailDataModel Getsystempropertyhouseagreementdetaildatabypropertyidandownerid(long Propertyid, long Ownertenantid);
        TenantAgreementDetailDataModel Getsystempropertyhouseroomagreementdetaildatabytenantid(long Propertytenantid);
        Genericmodel Registersystempropertyhouseagreementdata(string JsonData);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabyhouseid(long Propertyhouseid);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabyownerid(long Ownerid);
        Genericmodel Registersystempropertyhouseroomtenantdata(string JsonData);
        Genericmodel Registerpropertyhouseroomdata(string JsonData);
        Systempropertyhouseroomfixturesdata Getsystempropertyhouseroomfixturesdatabyhouseroomid(long Houseroomid);
        Genericmodel Registersystempropertyhouseroomfixturedata(string JsonData);
        Genericmodel Registersystempropertyhouseroomimagedata(string JsonData);
        SystemPropertyHouseImageData Getsystempropertyhouseroomimagebyhouseroomid(long Houseroomid);
        SystemPropertyHouseImageData Getsystempropertyhouseroomimagebyhouseid(long Houseid);
        Genericmodel Registerpropertyhousevacaterequestdata(string JsonData);
        Systempropertyhouseroomdata Getsystempropertyhouseroomdatabyid(long Houseroomid);
        Systempropertyhouseroommeters Getsystempropertyhouseroommeterdatabyid(long Houseroomid);
        Genericmodel Registerpropertyhouseroommeterdata(string JsonData);
        SystemPropertyHouseVacatingRequestModel Gettenantvacatingrequestsdatabyownerid(long Ownerid);
        Genericmodel Approvepropertyhousevacatingrequest(string JsonData);
        TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabyownerid(long Ownerid);
        TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabytenantid(long Tenantid);
        TenantMonthlyInvoiceDetailData Gettenantmonthlyinvoicedetaildatabyinvoiceid(long Invoiceid);
        Genericmodel Registerpropertyhouseroomrentpaymentrequestdata(string JsonData);
        TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabytenantid(long Tenantid);
        TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyownerid(long Ownerid);
        CustomerPaymentValidationData Getsystempropertyroompaymentbypaymentid(long Paymentid);
        Genericmodel Registervalidatecustomerpaymentrequestdata(string JsonData);
    }
}
