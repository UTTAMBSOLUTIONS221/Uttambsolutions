﻿using DBL.Entities;
using DBL.Models;
using DBL.Models.Dashboards;

namespace DBL.Repositories
{
    public interface IPropertyRepository
    {
        #region System Property Summary
        Maqaoplussummary Getmaqaoplussummarydata();
        #endregion

        #region System Property House Listing
        Genericmodel Registerhousepropertydata(string JsonData);
        #endregion


        PropertyHouseDetailData Getallsystempropertyvacanthousesdata(int Page, int PageSize);
        IEnumerable<Systemproperty> Getsystempropertyhousedata();
        Genericmodel Registersystempropertyhousedata(string JsonData);
        Genericmodel Registersystemagentpropertyhousedata(string JsonData);
        Systemproperty Getsystempropertyhousedatabyid(long Propertyid);
        SystemPropertyHouseCareTakerData Getsystempropertyhousecaretakerdatabyownerid(long Ownerid);
        SystemStaffData Getsystempropertyhousecaretakerdatabyid(long Caretakerhouseid);
        PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyowner(long Ownerid);
        PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyagent(long Agentid);
        Systempropertyhousedata Getsystempropertyhousedatabyowner(long Ownerid);
        Systempropertyhousedata Getallsystempropertyhousedata();
        Systempropertyhousedata Getsystempropertyhousedatabyagent(long Agentid);
        PropertyHouseRoomTenantModel Getsystempropertyhousetenantdatabytenantid(long TenantId);
        PropertyHouseTenantData Getsystempropertyhouseroomtenantsdata();
        PropertyHouseTenantData Getsystempropertyhouseroomtenantsdata(long Ownerid);
        PropertyHouseTenantData Getsystemagentpropertyhouseroomtenantsdata(long Agentid);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabypropertyidandownerid(long Propertyid, long Ownerid);
        OwnerTenantAgreementDetailDataModel Getsystempropertyhouseagreementdetaildatabyownerid(long Ownerid);
        OwnerTenantAgreementDetailDataModel Getsystempropertyhouseagreementdetaildatabyagentid(long Agentid);
        TenantAgreementDetailDataModel Getsystempropertyhouseroomagreementdetaildatabytenantid(long Tenantid);
        Genericmodel Registersystempropertyhouseagreementdata(string JsonData);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabyhouseid(long Propertyhouseid);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabyownerid(long Ownerid);
        Genericmodel Registersystempropertyhouseroomtenantdata(string JsonData);
        Genericmodel Registerpropertyhouseroomdata(string JsonData);
        Genericmodel Registersystempropertyhouseroommeterdata(string JsonData);
        Systempropertyhouseroomfixturesdata Getsystempropertyhouseroomfixturesdatabyhouseroomid(long Houseroomid);
        Genericmodel Registersystempropertyhouseroomfixturedata(string JsonData);
        Genericmodel Registersystempropertyhouseroomimagedata(string JsonData);
        SystemPropertyHouseImageData Getsystempropertyimagebyhouseroomid(long Houseroomid);
        SystemPropertyHouseImageData Getsystempropertyimagebyhouseid(long Houseid);
        Genericmodel Registerpropertyhousevacaterequestdata(string JsonData);
        Systempropertyhouseroomdata Getsystempropertyhouseroomdatabyid(long Houseroomid);
        Systempropertyhouseroommeters Getsystempropertyhouseroommeterdatabyid(long Houseroomid);
        SystemPropertyHouseVacatingRequestModel Gettenantvacatingrequestsdatabyownerid();
        SystemPropertyHouseVacatingRequestModel Gettenantvacatingrequestsdatabyownerid(long Ownerid);
        Genericmodel Approvepropertyhousevacatingrequest(string JsonData);
        TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabyownerid(long Ownerid);
        TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabyagentid(long Agentid);
        TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabytenantid(long Tenantid);
        TenantMonthlyInvoiceDetailData Gettenantmonthlyinvoicedetaildatabyinvoiceid(long Invoiceid);
        Genericmodel Registerpropertyhouseroomrentpaymentrequestdata(string JsonData);
        TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabytenantid(long Tenantid);
        TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyownerid();
        TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyownerid(long Ownerid);
        TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyagentid(long Agentid);
        CustomerPaymentValidationData Getsystempropertyroompaymentbypaymentid(long Paymentid);
        Genericmodel Registervalidatecustomerpaymentrequestdata(string JsonData);
        Genericmodel Updatemonthlyrentinvoicedata(long Invoiceid);
    }
}
