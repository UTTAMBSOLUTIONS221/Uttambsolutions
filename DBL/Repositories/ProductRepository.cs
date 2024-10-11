using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<Systemstoreitems> Getsystemstoreitemdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                return connection.Query<Systemstoreitems>("Usp_Getsystemstoreitemdata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registerstoreproductdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerstoreproductdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Systemstoreitems Getsystemstoreitemdatabyid(int Storeitemid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Storeitemid", Storeitemid);
                parameters.Add("@Storeitemdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemstoreitemdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string storeitemdataJson = parameters.Get<string>("@Storeitemdata");
                if (storeitemdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemstoreitems>(storeitemdataJson);
                }
                else
                {
                    return new Systemstoreitems();
                }
            }
        }
        public Genericmodel Registersystemproductdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemproductdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<Systemproducts> Getsystemproductdata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<Systemproducts>("Usp_Getsystemproductdata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Systemproducts Getsystemproductdatabyid(long Productid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Productid", Productid);
                return connection.Query<Systemproducts>("Usp_Getsystemproductdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
