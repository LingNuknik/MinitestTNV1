using Course.Core.Models;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Da
{
    public class ProductDa : IProductDa
    {
        private readonly CourseContext db;

        public ProductDa(CourseContext db)
        {
            this.db = db;
        }

        public async Task<List<ProductDT>> GetByCategoryId(int CategoryId)
        {
          
            var raw = from a in db.Products
                      join b in db.Category on a.CategoryId equals b.Id
                      where !a.IsDelete && !b.IsDelete && b.Id == CategoryId
                      select new ProductDT
                      {
                          
                                   Id = a.Id,
                                  Name = a.Name,
                                  Image = a.Image,
                                  Ranking = a.Ranking,
                                  Price = a.Price,
                                  PriceNet = a.PriceNet,
                                  CategoryName = b.Name,
                                  CategoryId = a.CategoryId,
                                  Discount = a.Discount,
                                  IsActive = a.IsActive,
                                  CreateBy = a.UpdateBy == null ? a.CreateBy : a.UpdateBy,
                                 CreateDate = a.UpdateDate == null ? a.CreateDate : a.UpdateDate.Value
                      };

            List<ProductDT> pro = new List<ProductDT>();
            foreach (var data in raw)
            {
                pro.Add(data);
            }



            return pro;
        }

        public async Task<Products> GetById(int id)
        {
            return  db.Products.FirstOrDefault(wh => wh.Id == id);
        }

        public async Task Insert(Products req)
        {
            await db.AddAsync(req);
            await db.SaveChangesAsync();

        }

        public async Task<Products> Update(Products data)
        {
            var o = db.Products.FirstOrDefault(wh => wh.Id == data.Id);

            if (o != null)
            {
                o.Name = data.Name;
                o.Image = data.Image;
                o.Discount = data.Discount;
                o.Price = data.Price;
                o.CategoryId = data.CategoryId;
                o.IsActive = data.IsActive;
                o.UpdateBy = data.UpdateBy;
                o.UpdateDate = data.UpdateDate;
            }

            await db.SaveChangesAsync();

            return o;

        }

        public async Task<Products> Delete(int id, string user)
        {
            var o = await GetById(id);


            Products p = new Products();
            p.Name = o.Name;
            p.Image= o.Image;
            p.Discount = o.Discount;
            p.Price = o.Price;
            p.CategoryId = o.CategoryId;
            p.IsActive = o.IsActive;
            p.Ranking = o.Ranking;
            p.IsDelete = true;
            p.UpdateDate = DateTime.Now;
            p.UpdateBy = user;

            await db.SaveChangesAsync();

            return p;
        }

        public async Task<int> GetNextRanking()
        {
            var x = await db.Products.Where(w => !w.IsDelete).OrderByDescending(u => u.Ranking).Select(s => s.Ranking).FirstOrDefaultAsync();

            return x + 1;
        }

        public async Task<Products> GetByName(string name) => db.Products.FirstOrDefault(f => f.Name == name && !f.IsDelete);

        public async Task<getProductDTRes> getDataDT(getProductDTReq req)
        {
            var raw = from a in db.Products
                      join b in db.Category on a.CategoryId equals b.Id
                      where !a.IsDelete && !b.IsDelete
                      select new ProductDT
                      {                          
                                 Id = a.Id,
                                 Name = a.Name,
                                 Image = a.Image,
                                 Ranking = a.Ranking,
                                 Price = a.Price,
                                 PriceNet = a.PriceNet,
                                 CategoryName = b.Name,
                                 CategoryId = a.CategoryId,
                                 Discount = a.Discount,
                                 IsActive = a.IsActive,
                          CreateBy = a.UpdateBy == null ? a.CreateBy : a.UpdateBy,
                          CreateDate = a.UpdateDate == null ? a.CreateDate : a.UpdateDate.Value

                      };



            if (!string.IsNullOrEmpty(req.Name))
                raw = raw.Where(wh => wh.Name.Contains(req.Name));

            int count = await raw.CountAsync();

            if (req.OrderType == "asc")
            {
                if (req.OrderName == "Name")
                    raw = raw.OrderBy(w => w.Name);
                else if (req.OrderName == "IsActive")
                    raw = raw.OrderBy(w => w.IsActive);
                else if (req.CategoryId == "CategoryId")
                    raw = raw.OrderBy(w => w.CategoryId);
            }
            else if (req.OrderType == "desc")
            {
                if (req.OrderName == "Name")
                    raw = raw.OrderByDescending(w => w.Name);
                else if (req.CategoryId == "CategoryId")
                    raw = raw.OrderBy(w => w.CategoryId);
            }

            if(count == 0)
                req.Length = count;

            var data = await raw.Skip(req.Start).Take(req.Length).Select(a => new ProductDT
            {
                Id = a.Id,
                Name = a.Name,
                Image = a.Image,
                Ranking = a.Ranking,
                Price = a.Price,
                Discount = a.Discount,
                CategoryName = a.CategoryName,
                CategoryId = a.CategoryId,
                PriceNet = a.PriceNet,
                IsActive = a.IsActive,
                CreateBy = a.CreateBy,
                CreateDate = a.CreateDate
            }).ToListAsync();



            return new getProductDTRes
            {
                data = data,
                draw = req.draw,
                recordsFiltered = count,
                recordsTotal = count
            };
        }
    }
}
