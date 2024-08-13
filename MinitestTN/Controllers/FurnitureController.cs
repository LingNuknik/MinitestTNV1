using Course.Core.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MinitestTN.Common;

namespace MinitestTN.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ICategoryDa iCategoryDa;
        private readonly IProductDa iProductDa;
        private readonly FileHelper fileHelper;
        private readonly string folderCategory = "Category";
        private readonly string folderProduct = "Products";


        public FurnitureController(IConfiguration configuration, ICategoryDa iCategoryDa, FileHelper fileHelper, IProductDa iProductDa)
        {
            this.iCategoryDa = iCategoryDa;
            this.fileHelper = fileHelper;
            this.iProductDa = iProductDa;
        }
        public async Task<IActionResult> Index()
        {
            getCategoryDTReq req = new getCategoryDTReq();

            ViewBag.DataCat = await iCategoryDa.getDataCatDT(req);

            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

       


    }
}
