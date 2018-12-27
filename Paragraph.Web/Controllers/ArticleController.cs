using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Paragraph.Web.Controllers
{
    using Paragraph.Services.DataServices.Models.Article;
    using Services.DataServices;


    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
        }

        public IActionResult Details(int id)
        {
            var joke = this.articleService.GetArticleById(id);
            return this.View(joke);
        }

        [Authorize]
        public IActionResult Create()
        {
            this.ViewData["Categories"] = categoryService.GetCategories()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                });
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateArticleInputModel model)
        {
           if(!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = this.User.Identity.Name;
            var id = this.articleService.Create(model, user);
            return this.RedirectToAction("Details", new { id = id});
        }

        
    }
}