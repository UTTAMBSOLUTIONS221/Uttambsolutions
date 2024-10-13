using Dapper;
using DBL.Entities;
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

        #region Collection centers
        public IEnumerable<Parcelcollectioncenters> Getparcelcollectioncentersdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Parcelcollectioncenters>("Usp_Getparcelcollectioncentersdata", parameters, commandType: CommandType.StoredProcedure).ToList();
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
    }
}
