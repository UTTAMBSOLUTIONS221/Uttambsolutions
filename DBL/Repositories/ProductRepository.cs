using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            Systemstoreitems Storeitemdata = new Systemstoreitems();
            List<Storeproductimages> Storeproductimages = new List<Storeproductimages>();
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
                    JObject responseJson = JObject.Parse(storeitemdataJson);
                    Storeitemdata.Storeitemid = Convert.ToInt32(responseJson["Storeitemid"]);
                    Storeitemdata.Storeitemname = responseJson["Storeitemname"].ToString();
                    Storeitemdata.Itembrandname = responseJson["Itembrandname"].ToString();
                    Storeitemdata.Itemsize = responseJson["Itemsize"].ToString();
                    Storeitemdata.Itembuyingprice = Convert.ToDecimal(responseJson["Itembuyingprice"]);
                    Storeitemdata.Itemsellingprice = Convert.ToDecimal(responseJson["Itemsellingprice"]);
                    Storeitemdata.Itemstatus = Convert.ToInt32(responseJson["Itemstatus"]);
                    Storeitemdata.Storeproductimgurl = responseJson["Storeproductimgurl"].ToString();
                    Storeitemdata.Productavailability = responseJson["Productavailability"].ToString();
                    Storeitemdata.Productcondition = responseJson["Productcondition"].ToString();
                    Storeitemdata.Productstatus = responseJson["Productstatus"].ToString();
                    Storeitemdata.Isactive = Convert.ToBoolean(responseJson["Isactive"]);
                    Storeitemdata.Isdeleted = Convert.ToBoolean(responseJson["Isdeleted"]);
                    Storeitemdata.Createdby = Convert.ToInt32(responseJson["Createdby"]);
                    Storeitemdata.Modifiedby = Convert.ToInt32(responseJson["Modifiedby"]);
                    Storeitemdata.Datecreated = Convert.ToDateTime(responseJson["Datecreated"]);
                    Storeitemdata.Datemodified = Convert.ToDateTime(responseJson["Datemodified"]);
                    if (responseJson["Storeproductimages"] != null)
                    {
                        string StoreproductimagesJson = responseJson["Storeproductimages"].ToString();
                        Storeproductimages = JsonConvert.DeserializeObject<List<Storeproductimages>>(StoreproductimagesJson);
                        Storeitemdata.Storeproductimages = Storeproductimages;
                    }
                    return Storeitemdata;
                }
                else
                {
                    return Storeitemdata;
                }
            }
        }
        public Systemstoreitemsdata Getallsystemstoreitemdata()
        {
            Systemstoreitemsdata Storeitemdata = new Systemstoreitemsdata();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Storeitemdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getallsystemstoreitemdata", parameters, commandType: CommandType.StoredProcedure);
                string storeitemdataJson = parameters.Get<string>("@Storeitemdata");
                if (storeitemdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemstoreitemsdata>(storeitemdataJson);
                }
                else
                {
                    return new Systemstoreitemsdata();
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
