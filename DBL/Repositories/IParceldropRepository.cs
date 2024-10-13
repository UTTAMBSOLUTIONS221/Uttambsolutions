using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IParceldropRepository
    {
        #region Collection Centers
        IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersdata();
        Genericmodel Registerparcelcollectioncenterdata(string JsonData);
        Parcelcollectioncenters Getparcelcollectioncentersdatabyid(int Collectioncenterid);
        #endregion
    }
}
