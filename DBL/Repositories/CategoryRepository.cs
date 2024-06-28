using Dapper;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBL.Entities;

namespace DBL.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(string connectionString) : base(connectionString)
        {
        }
        public Genericmodel Registersystemcategorydata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemcategorydata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<Productcategories> Getsystemcategorydata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<Productcategories>("Usp_Getsystemcategorydata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Productcategories Getsystemcategorydatabyid(long Categoryid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Categoryid", Categoryid);
                return connection.Query<Productcategories>("Usp_Getsystemcategorydatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
