using Course.Core.Models;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Da
{
    public class CategoryDa : ICategoryDa
    {
        private readonly CourseContext db;

        public CategoryDa(CourseContext db)
        {
            this.db = db;
        }

        public async Task<Category> GetById(int id)
        {
            return await db.Category.FindAsync(id);
        }

        public async Task Insert(Category req)
        {
            await db.AddAsync(req);
            await db.SaveChangesAsync();

        }

        public async Task<Category> isExist(Category data) => db.Category.FirstOrDefault(wh => wh.Name == data.Name && !wh.IsDelete);

        public async Task<Category> UpdateCat(Category data)
        {
            var o = db.Category.FirstOrDefault(wh => wh.Id == data.Id);

            if (o != null)
            {
                o.Name = data.Name;
                o.Image = data.Image;
                o.IsActive = data.IsActive;
                o.UpdateBy = data.UpdateBy;
                o.UpdateDate = data.UpdateDate;
            }

           await db.SaveChangesAsync();

            return o;

        }

        public async Task<Category> Delete(int id, string user)
        {
            var o = await GetById(id);
            o.IsDelete = true;
            o.UpdateDate = DateTime.Now;
            o.UpdateBy = user;

            await db.SaveChangesAsync();

            return o;
        }

        public async Task<int> GetNextRanking()
        {
            var x = await db.Category.Where(w => !w.IsDelete).OrderByDescending(u => u.Ranking).Select(s => s.Ranking).FirstOrDefaultAsync();

            return x + 1;
        }

        public async Task<Category> GetByName(string name) => db.Category.FirstOrDefault(f => f.Name == name && !f.IsDelete);


        public async Task<getCategoryDTRes> getDataCatDT(getCategoryDTReq req)
        {
            var raw = db.Category.Where(w => !w.IsDelete);

            if (!string.IsNullOrEmpty(req.Name))
                raw = raw.Where(wh => wh.Name.Contains(req.Name));

            int count = await raw.CountAsync();

            if (req.OrderType == "asc")
            {
                if (req.OrderName == "Name")
                    raw = raw.OrderBy(w => w.Name);
                else if (req.OrderName == "IsActive")
                    raw = raw.OrderBy(w => w.IsActive);
            }
            else if (req.OrderType == "desc")
            {
                if (req.OrderName == "Name")
                    raw = raw.OrderByDescending(w => w.Name);
            }

            if (req.Length == 0)
                req.Length = raw.Count();

            var data = await raw.Skip(req.Start).Take(req.Length).Select(a => new CategoryDT
            {
                Id = a.Id,
                Name = a.Name,
                Image = a.Image,
                Ranking = a.Ranking,
                IsActive = a.IsActive,
                CreateBy = a.UpdateBy == null ? a.CreateBy : a.UpdateBy
              ,
                CreateDate = a.UpdateDate == null ? a.CreateDate : a.UpdateDate.Value
            }).ToListAsync();



            return new getCategoryDTRes
            {
                data = data,
                draw = req.draw,
                recordsFiltered = count,
                recordsTotal = count
            };
        }

    }
}
