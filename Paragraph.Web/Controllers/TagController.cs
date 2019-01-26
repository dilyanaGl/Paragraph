using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Paragraph.Web.Controllers
{
    using Services.DataServices;
    using Services.DataServices.Models.Tag;

    public class TagController : Controller
    {
        private readonly ITagService tagService;
        private readonly IArticleService articleService;

        public TagController(ITagService tagService, IArticleService articleService)
        {
            this.tagService = tagService;
            this.articleService = articleService;
        }   
     
        public IActionResult Details(int id)
        {
            if(!tagService.IsTagValid(id))
            {
                return this.RedirectToAction("All", "Tag");
            }

            var tag = tagService.Details(id);
            return this.View(tag);
        }

        public IActionResult All()
        {
            var model = this.tagService.All();

            return this.View(model);
        }

        [Authorize(Roles="Admin, Moderator")]
        public IActionResult Create() => this.View();

        [Authorize(Roles ="Admin, Moderator")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(CreateTagModel tag)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View();
            }
            this.tagService.Create(tag);
            return this.RedirectToAction("All", "Tag");
        }

        [Authorize(Roles ="Admin, Moderator")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddTag(AddTagModel model, int id)
        {
            if(!this.articleService.DoesArticleExist(id))
            {
                this.ViewData["Error"] = "Article does not exist.";
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Article", new { id = id });
            }
            this.tagService.AddTagToArticle(model.TagId, id);
            return this.RedirectToAction("Details", "Article", new { id = id});
        }

      

    }
}