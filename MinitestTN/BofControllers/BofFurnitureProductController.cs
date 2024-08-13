using Course.Core.Models;
using DataAccess.Da;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MinitestTN.Common;

namespace MinitestTN.BofControllers
{
    public class BofFurnitureProductController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IProductDa iProductDa;
        private readonly FileHelper fileHelper;
        private readonly string folderName = "Products";

        public BofFurnitureProductController(IConfiguration configuration, IProductDa iProductDa, FileHelper fileHelper)
        {
            this.iProductDa = iProductDa;
            this.fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll(getProductDTReq req)
        {

            var data = await iProductDa.getDataDT(req);

            return Json(data);
        }

        public async Task<IActionResult> GetNextRanking()
        {
            var data = await iProductDa.GetNextRanking();

            return Json(data);
        }

        public async Task<IActionResult> GetByID(int id)
        {
            var data = await iProductDa.GetById(id);

            return Json(data);
        }

        public async Task<IActionResult> Update(Products data, IFormFile file1)
        {
            CommonRes res = new();
            try
            {
                if (file1 != null)
                {
                    if (data.Image != null)
                        fileHelper.Delete(folderName, data.Image);

                    data.Image = await fileHelper.Upload(file1, folderName);
                }

                data.UpdateBy = User.Identity.Name;
                data.UpdateDate = DateTime.Now;

                await iProductDa.Update(data);


                res.Success = true;


            }
            catch (Exception ex)
            {
                res.Message = $"ERROR: {ex.Message}";
            }
            return Json(res);
        }

        public async Task<IActionResult> Delete(int id)
        {
            CommonRes res = new();
            try
            {
                var o = await iProductDa.Delete(id, User.Identity.Name);

                fileHelper.Delete(folderName, o.Image);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = $"ERROR: {ex.Message}";
            }
            return Json(res);


        }

        public async Task<IActionResult> Insert(Products data, IFormFile file1)
        {
            CommonRes res = new();
            try
            {
                var x = await iProductDa.GetByName(data.Name);
                if (x == null)
                {
                    data.Image = await fileHelper.Upload(file1, folderName);
                    data.CreateDate = DateTime.Now;
                    data.UpdateBy = User.Identity.Name;

                    await iProductDa.Insert(data);

                    res.Success = true;
                    res.Message = "Success.";
                }
                else
                {
                    res.Message = $"{data.Name} is duplicated.";
                }
            }
            catch (ArgumentException ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"ERROR: {ex.Message}";
            }
            return Json(res);

        }

    }
}
