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
        private readonly ITagService tagService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, ICommentService commentService, ITagService tagService)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.commentService = commentService;
            this.tagService = tagService;
        }

       
        public IActionResult Details(int id)
        {
            var model = this.articleService.GetArticleById(id);
            this.ViewData["Tags"] = this.tagService.TagsForArticle(id).Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });
            return this.View(model);
        }

        [Authorize(Roles ="Admin, Writer")]
        public IActionResult Create()
        {
            this.ViewData["Categories"] = categoryService.GetCategories()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                });

            this.ViewData["Tags"] = tagService.All()
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

        [Authorize(Roles ="Admin, Writer")]
        [HttpPost]
        public IActionResult Create(CreateArticleInputModel model)
        {
           if(!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = this.User.Identity.Name;
           this.articleService.Create(model, user);
            return this.Redirect("/home/index");
        }

        [Authorize(Roles ="Admin, Writer")]
        public IActionResult Edit(int id)
        {
            var model = this.articleService.GetArticleById(id);
            return this.View(model);
        }

        [Authorize(Roles ="Admin, Writer")]
        [HttpPost]
        public IActionResult Edit(ArticleViewModel model)
        {
            this.articleService.Edit(model);
            return this.RedirectToAction("Details", new { id = model.Id});        }


        //[HttpGet]
        //[Authorize(Roles ="Admin")]
        //public IActionResult Delete(int id)
        //{
        //    var model = this.articleService.GetArticleById(id);
        //    return this.View(model);
        //}

     

        //[HttpPost]
        //[Authorize(Roles ="Admin")]
        //public IActionResult Delete(ArticleViewModel model)
        //{
        //    this.articleService.Delete(model.Id);
        //    return this.Redirect("/Home/Index");
        //}
    }
}