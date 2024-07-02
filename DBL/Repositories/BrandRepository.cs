using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class BrandRepository : BaseRepository, IBrandRepository
    {
        public BrandRepository(string connectionString) : base(connectionString)
        {
        }
        public Genericmodel Registersystemproductbranddata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemproductbranddata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<Productbrand> Getsystemproductbranddata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<Productbrand>("Usp_Getsystemproductbranddata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Productbrand Getsystemproductbranddatabyid(long Brandid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Brandid", Brandid);
                return connection.Query<Productbrand>("Usp_Getsystemproductbranddatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
