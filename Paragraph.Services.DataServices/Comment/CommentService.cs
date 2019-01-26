using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Paragraph.Services.DataServices
{
    using Data.Common;
    using Data.Models;
    using Models.Comment;
    using AutoMapper;
    using Services.Mapping;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentRepository;
        private readonly IRepository<ParagraphUser> userRepository;
        private readonly IRepository<Article> articleRepository; 

        public CommentService(IRepository<Comment> commentRepository, IRepository<ParagraphUser> userRepository, IRepository<Article> articleRepository)
        {
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
            this.articleRepository = articleRepository;
        }

        
        public void AddComment(AddCommentModel model, string username, int articleId)
        {
            // TODO: Check why comments are not inserted into the database

            var author = this.userRepository.All().Where(p => p.UserName == username).FirstOrDefault();
            var article = this.articleRepository.All().SingleOrDefault(p => p.Id == articleId);
            var comment = new Comment
            {
                Content = model.Content,
                Author = author,
                Article = article
            };

            this.commentRepository.AddAsync(comment);
            this.commentRepository.SaveChangesAsync();
            
        }

        public bool DoesCommentExist(int id)
        {
            return this.commentRepository.All().Any(p => p.Id == id);
        }

        public IEnumerable<CommentViewModel> DisplayComments(int articleId)
        {
            var comments = this.commentRepository.All().Where(p => p.Article.Id == articleId)
                .OrderByDescending(p => p.PublishedOn)
                .To<CommentViewModel>()
                .ToArray();

            return comments;
        }

        public void Delete(int id)
        {
            var comment = this.commentRepository.All().SingleOrDefault(p => p.Id == id);
            this.commentRepository.Delete(comment);
            this.commentRepository.SaveChangesAsync();
        }
    }
}
