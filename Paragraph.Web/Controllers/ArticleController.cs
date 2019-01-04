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
        private readonly ICommentService commentService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, ICommentService commentService)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.commentService = commentService;
        }

       
        public IActionResult Details(int id)
        {
            var model = this.articleService.GetArticleById(id);
            return this.View(model);
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

        public IActionResult All()
        {
            var articles = this.articleService.All();
            return this.View(articles);
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
            var id = this.articleService.Create(model, user).Result;
            return this.Redirect("/home/index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var model = this.articleService.GetArticleById(id);
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(ArticleViewModel model)
        {
            this.articleService.Edit(model);
            return this.RedirectToAction("Details", new { id = model.Id});        }


        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var model = this.articleService.GetArticleById(id);
            return this.View(model);
        }

     

        [HttpPost]
        [Authorize]
        public IActionResult Delete(ArticleViewModel model)
        {
            this.articleService.Delete(model.Id);
            return this.Redirect("/Home/Index");
        }
    }
}