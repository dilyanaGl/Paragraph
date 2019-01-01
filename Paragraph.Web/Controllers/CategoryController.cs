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

    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;    

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
            
        }

        [Authorize]
        public IActionResult All()
        {
            var categories = this.categoryService.ListCategoriesAndCount();

            return View(categories);
        }

        [Authorize]
        public IActionResult Articles(int id)
        {
            var model = this.categoryService.GetCategoryWithArticles(id);
            return this.View(model);
        }
    }
}