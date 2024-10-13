using DBL.Entities;

namespace DBL.Repositories
{
    public interface IParceldropRepository
    {
        #region Collection Centers
        IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersdata();
        Parcelcollectioncenters Getparcelcollectioncentersdatabyid(int Collectioncenterid);
        #endregion
    }
}
