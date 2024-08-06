using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IPropertyRepository
    {
        IEnumerable<Systemproperty> Getsystempropertyhousedata();
        Genericmodel Registersystempropertyhousedata(string JsonData);
        Systemproperty Getsystempropertyhousedatabyid(long Propertyid);
        Systempropertyhousedata Getsystempropertyhousedatabyowner(long Ownerid);
        PropertyHouseRoomTenantModel Getsystempropertyhousetenantdatabytenantid(long TenantId);
        PropertyHouseDetailData Getsystempropertyhousedetaildatabypropertyidandownerid(long Propertyid, long Ownerid);
        Genericmodel Registersystempropertyhouseroomtenantdata(string JsonData);
        Genericmodel Registerpropertyhouseroomdata(string JsonData);
        Systempropertyhouserooms Getsystempropertyhouseroomdatabyid(long Houseroomid);
        Systempropertyhouseroommeters Getsystempropertyhouseroommeterdatabyid(long Houseroomid);
        Genericmodel Registerpropertyhouseroommeterdata(string JsonData);
    }
}
