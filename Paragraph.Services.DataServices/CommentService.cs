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

        
        public async void AddComment(AddCommentModel model, string username, int articleId)
        {
            var author = this.userRepository.All().Where(p => p.UserName == username).FirstOrDefault();
            var article = this.articleRepository.All().SingleOrDefault(p => p.Id == articleId);
            var comment = new Comment
            {
                Content = model.Content,
                Author = author,
                Article = article
            };

            await this.commentRepository.AddAsync(comment);
            await this.commentRepository.SaveChangesAsync();
            
        }

        public IEnumerable<CommentViewModel> DisplayComments(int articleId)
        {
            var comments = this.commentRepository.All().Where(p => p.Article.Id == articleId)
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
