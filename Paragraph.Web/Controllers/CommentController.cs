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

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult AddComment(AddCommentModel model, int articleId)
        {
            this.commentService.AddComment(model, this.User.Identity.Name, articleId);
            return this.RedirectToAction("Details", "Article", new { Id = articleId });
        }

        
        [Authorize(Roles ="Admin, Moderator")]
        public IActionResult Delete(int commentId, int articleId)
        {
            this.commentService.Delete(commentId);
            return this.RedirectToAction("Details", "Article", new { Id = articleId });
            //return this.RedirectToAction("Index", "Home");
        }
        
    }
}