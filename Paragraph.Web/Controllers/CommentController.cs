using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Paragraph.Web.Controllers
{
    using Services.DataServices;
    using Services.DataServices.Models.Comment;

    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IArticleService articleService;

        public CommentController(ICommentService commentService, IArticleService articleService)
        {
            this.commentService = commentService;
            this.articleService = articleService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(AddCommentModel model, int articleId)
        {
            if (this.articleService.DoesArticleExist(articleId))
            {
                this.ViewData["Error"] = "Article does not exist.";
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Article", new { Id = articleId });
            }
            this.commentService.AddComment(model, this.User.Identity.Name, articleId);
            return this.RedirectToAction("Details", "Article", new { Id = articleId });
        }


        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Delete(int commentId, int articleId)
        {

            if (!this.articleService.DoesArticleExist(articleId))
            {
                this.ViewData["Error"] = "Article does not exist.";
                this.RedirectToAction("Index", "Home");
            }

            if (!this.commentService.DoesCommentExist(commentId))
            {
                return this.RedirectToAction("Details", "Article", new { Id = articleId });
            }

            this.commentService.Delete(commentId);
            return this.RedirectToAction("Details", "Article", new { Id = articleId });
            //return this.RedirectToAction("Index", "Home");
        }

    }
}