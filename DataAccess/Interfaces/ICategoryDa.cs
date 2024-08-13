using Course.Core.Models;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ICategoryDa
    {
        Task<Category> GetById(int id);
        Task<getCategoryDTRes> getDataCatDT(getCategoryDTReq req);
        Task Insert(Category req);
        Task<Category> isExist(Category data);
        Task<Category> UpdateCat(Category data);
        Task<int> GetNextRanking();
        Task<Category> GetByName(string name);
        Task<Category> Delete(int id, string user);
    }
}
