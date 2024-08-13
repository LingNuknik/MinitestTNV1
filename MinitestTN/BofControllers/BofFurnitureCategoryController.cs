using Course.Core.Models;
using DataAccess.Da;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MinitestTN.Common;

namespace MinitestTN.BofControllers
{
    public class BofFurnitureCategoryController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ICategoryDa iCategoryDa;
        private readonly FileHelper fileHelper;
        private readonly string folderName = "Category";


        public BofFurnitureCategoryController(IConfiguration configuration, ICategoryDa iCategoryDa, FileHelper fileHelper)
        {
            this.iCategoryDa = iCategoryDa;
            this.fileHelper = fileHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll(getCategoryDTReq req)
        {

            var data = await iCategoryDa.getDataCatDT(req);

            return Json(data);
        }

        public async Task<IActionResult> GetNextRanking()
        {
            var data = await iCategoryDa.GetNextRanking();

            return Json(data);
        }

        public async Task<IActionResult> GetCatByID(int id)
        {
            var data = await iCategoryDa.GetById(id);

            return Json(data);
        }

        public async Task<IActionResult> UpdateCat(Category data, IFormFile file1)
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

             await iCategoryDa.UpdateCat(data);

               
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
                var o = await iCategoryDa.Delete(id, User.Identity.Name);

                fileHelper.Delete(folderName, o.Image);

                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = $"ERROR: {ex.Message}";
            }
            return Json(res);


        }

        public async Task<IActionResult> Insert(Category data, IFormFile file1)
        {
            CommonRes res = new();
            try
            {
                var x = await iCategoryDa.GetByName(data.Name);
                if (x == null)
                {
                    data.Image = await fileHelper.Upload(file1, folderName);
                    data.CreateDate = DateTime.Now;
                    data.UpdateBy = User.Identity.Name;

                    await iCategoryDa.Insert(data);

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
