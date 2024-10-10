﻿using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IProductRepository
    {
        Genericmodel Registerstoreproductdata(string JsonData);
        Genericmodel Registersystemproductdata(string JsonData);
        IEnumerable<Systemproducts> Getsystemproductdata(int Page, int PageSize);
        Systemproducts Getsystemproductdatabyid(long Productid);
    }
}
