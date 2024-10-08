﻿using Course.Core.Models;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IProductDa
    {
        Task<Products> GetById(int id);
        Task<List<ProductDT>> GetByCategoryId(int CategoryId);
        Task Insert(Products req);
        Task<Products> Update(Products data);
        Task<Products> Delete(int id, string user);
        Task<int> GetNextRanking();
        Task<getProductDTRes> getDataDT(getProductDTReq req);
        Task<Products> GetByName(string name);
    }
}
