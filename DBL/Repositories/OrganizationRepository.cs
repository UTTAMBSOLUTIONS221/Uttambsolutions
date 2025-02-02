﻿using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        public OrganizationRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<SystemOrganization> Getsystemorganizationdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<SystemOrganization>("Usp_Getsystemorganizationdata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersystemorganizationdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemorganizationdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SystemOrganization Getsystemorganizationdatabyid(long Organizationid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Organizationid", Organizationid);
                return connection.Query<SystemOrganization>("Usp_Getsystemorganizationdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SystemOrganizationDetails Getsystemorganizationdetaildatabyid(long Organizationid)
        {

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Organizationid", Organizationid);
                parameters.Add("@OrganizationDetailData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemorganizationdetaildatabyid", parameters, commandType: CommandType.StoredProcedure);
                string organizationDetailDataJson = parameters.Get<string>("@OrganizationDetailData");
                if (organizationDetailDataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemOrganizationDetails>(organizationDetailDataJson);
                }
                else
                {
                    return new SystemOrganizationDetails();
                }
            }
        }
        public Organizationshopproductsdata Registerorganizationshopproductdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                parameters.Add("@Organizationshopproductsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Registerorganizationshopproductdata", parameters, commandType: CommandType.StoredProcedure);
                string shopproductDetailDataJson = parameters.Get<string>("@Organizationshopproductsdata");
                if (shopproductDetailDataJson != null)
                {
                    return JsonConvert.DeserializeObject<Organizationshopproductsdata>(shopproductDetailDataJson);
                }
                else
                {
                    return new Organizationshopproductsdata();
                }
            }
        }
        public Organizationshopproducts Getorganizationshopproductdatabyid(long Shopproductid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Shopproductid", Shopproductid);
                parameters.Add("@ShopproductDetailData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemorganizationshopproductdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string shopproductDetailDataJson = parameters.Get<string>("@ShopproductDetailData");
                if (shopproductDetailDataJson != null)
                {
                    return JsonConvert.DeserializeObject<Organizationshopproducts>(shopproductDetailDataJson);
                }
                else
                {
                    return new Organizationshopproducts();
                }
            }
        }

        public Systemorganizationshopproducts Getsystemorganizationshopproductsdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Organizationshopproductsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemorganizationshopproductsdata", parameters, commandType: CommandType.StoredProcedure);
                string organizationshopproductsdataJson = parameters.Get<string>("@Organizationshopproductsdata");
                if (organizationshopproductsdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemorganizationshopproducts>(organizationshopproductsdataJson);
                }
                else
                {
                    return new Systemorganizationshopproducts();
                }
            }
        }
        public Organizationshopproductsdata Getsystemorganizationshopproductsdatabyid(long Shopproductid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Shopproductid", Shopproductid);
                parameters.Add("@Organizationshopproductsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemorganizationshopproductsdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string organizationshopproductsdataJson = parameters.Get<string>("@Organizationshopproductsdata");
                if (organizationshopproductsdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Organizationshopproductsdata>(organizationshopproductsdataJson);
                }
                else
                {
                    return new Organizationshopproductsdata();
                }
            }
        }
    }
}
