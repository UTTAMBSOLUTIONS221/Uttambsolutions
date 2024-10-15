using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IParceldropRepository
    {
        #region Parcel Collection Centers
        IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersdata();
        IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersdata(int Managerid);
        Genericmodel Registerparcelcollectioncenterdata(string JsonData);
        Parcelcollectioncenters Getparcelcollectioncentersdatabyid(int Collectioncenterid);
        #endregion

        #region  Collection Center Parcels
        IEnumerable<Collectioncenterparcels> Getcollectioncenterparcelsdata();
        Genericmodel Registercollectioncenterparceldata(string JsonData);
        Collectioncenterparcels Getcollectioncenterparcelsdatabyid(int Parcelid);
        Genericmodel Registerparcelpaymentdata(string JsonData);
        #endregion


        #region Collection Center Courier
        Collectioncentercouriers Checkifcourierexistincollectioncenter(int Courierid);
        IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersnotindata(int Courierid);
        Genericmodel Registercollectioncentercourierdata(string JsonData);
        Genericmodel Registerparcelassignedtocourierdata(string JsonData);
        #endregion
    }
}
