using DBL.Entities;
using DBL.Models;
using DBL.Models.Dashboards;

namespace DBL.Repositories
{
    public interface IPropertyRepository
    {
        IEnumerable<Systemproperty> Getsystempropertyhousedata();
        Genericmodel Registersystempropertyhousedata(string JsonData);
        Systemproperty Getsystempropertyhousedatabyid(long Propertyid);
        PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyowner(long Ownerid, long Posterid);
        Systempropertyhousedata Getsystempropertyhousedatabyowner(long Ownerid);
        PropertyHouseRoomTenantModel Getsystempropertyhousetenantdatabytenantid(long TenantId);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabypropertyidandownerid(long Propertyid, long Ownerid);
        Genericmodel Registersystempropertyhouseroomtenantdata(string JsonData);
        Genericmodel Registerpropertyhouseroomdata(string JsonData);
        Systempropertyhouseroomdata Getsystempropertyhouseroomdatabyid(long Houseroomid);
        Systempropertyhouseroommeters Getsystempropertyhouseroommeterdatabyid(long Houseroomid);
        Genericmodel Registerpropertyhouseroommeterdata(string JsonData);
    }
}
