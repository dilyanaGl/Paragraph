using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Paragraph.Web.Controllers
{
    using Services.DataServices;
    using Services.DataServices.Models.Category;

    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;    

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
            
        }
        
        public IActionResult All()
        {
            var categories = this.categoryService.ListCategoriesAndCount();

            return View(categories);
        }
                
        public IActionResult Articles(int id)
        {
            if(!this.categoryService.DoesCategoryExist(id))
            {
                this.ViewData["Error"] = "Category does not exist.";
                return this.All();
            }
            var model = this.categoryService.GetCategoryWithArticles(id);
            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles ="Admin, Moderator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles ="Admin, Moderator")]
        public IActionResult Create(AddCategoryModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            this.categoryService.AddCategory(model);
            return this.RedirectToAction("All", "Category");
        }

    }
}