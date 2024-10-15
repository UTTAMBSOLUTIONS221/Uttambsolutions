using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class ParceldropRepository : BaseRepository, IParceldropRepository
    {
        public ParceldropRepository(string connectionString) : base(connectionString)
        {
        }

        #region Parcel Collection centers
        public IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Parcelcollectioncenters>("Usp_Getparcelcollectioncentersdata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registerparcelcollectioncenterdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerparcelcollectioncenterdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Parcelcollectioncenters Getparcelcollectioncentersdatabyid(int Collectioncenterid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Collectioncenterid", Collectioncenterid);
                return connection.Query<Parcelcollectioncenters>("Usp_Getparcelcollectioncentersdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion


        #region  Collection center Parcels
        public IEnumerable<Collectioncenterparcels> Getcollectioncenterparcelsdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Collectioncenterparcels>("Usp_Getcollectioncenterparcelsdata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registercollectioncenterparceldata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registercollectioncenterparceldata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Collectioncenterparcels Getcollectioncenterparcelsdatabyid(int Parcelid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Parcelid", Parcelid);
                return connection.Query<Collectioncenterparcels>("Usp_Getcollectioncenterparcelsdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion

        #region Collection center Couriers
        public Collectioncentercouriers Checkifcourierexistincollectioncenter(int Courierid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Courierid", Courierid);
                return connection.Query<Collectioncentercouriers>("Usp_Checkifcourierexistincollectioncenter", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersnotindata(int Courierid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Courierid", Courierid);
                return connection.Query<Parcelcollectioncenters>("Usp_Getparcelcollectioncentersnotindata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Genericmodel Registercollectioncentercourierdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registercollectioncentercourierdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Registerparcelassignedtocourierdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerparcelassignedtocourierdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion
    }
}
