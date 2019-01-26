using System;
using System.Collections.Generic;
using System.Linq;


namespace Paragraph.Services.DataServices
{
    using Data.Models;
    using Paragraph.Data.Common;
    using Models.Home;
    using Models.Article;
    using Models.Comment;
    using Models.Tag;
    using System.Threading.Tasks;
    using Paragraph.Services.Mapping;
    

    public class ArticleService : IArticleService
    {
        private readonly IRepository<Data.Models.Article> articleRepository;
        private readonly IRepository<Data.Models.Category> categoryRepository;
        private readonly IRepository<Data.Models.ParagraphUser> userRepository;
        private readonly IRepository<ArticleTag> articleTagRepository;
        private readonly IRepository<Tag> tagRepository;

        private readonly IRepository<Comment> commentRepository;

        public ArticleService(IRepository<Paragraph.Data.Models.Article> articleRepository, IRepository<Data.Models.Category> categoryRepository, IRepository<ParagraphUser> userRepository, IRepository<Comment> commentRepository, IRepository<Tag> tagRepository, IRepository<ArticleTag> articleTagRepository)
        {
            this.articleRepository = articleRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
            this.tagRepository = tagRepository;
            this.articleTagRepository = articleTagRepository;
        }

        public bool DoesArticleExist(int id)
        {
            return this.articleRepository.All().Any(p => p.Id == id);
        }

        public IEnumerable<ArticleIdAndName> All()
        {
            return this.articleRepository.All()
                .To<ArticleIdAndName>()
                .ToArray();
        }

        public IndexViewModel GetArticles(int num)
        {
            var allArticles = articleRepository.All();
            var articles =  allArticles.Take(num)
                .To<IndexArticleViewModel>()
                .ToArray();

            var model = new IndexViewModel { Articles = articles };

            return model;
        }

        public int Create(CreateArticleInputModel model, string username)
        {
            var category = this.categoryRepository.All().SingleOrDefault(p => p.Id == model.CategoryId);
            var author = this.userRepository.All().FirstOrDefault(p => p.UserName == username);
           
            
            var article = new Data.Models.Article
            {
                Title = model.Title,
                Content = model.Content,
                CategoryId = model.CategoryId,
                Author = author,
                

            };           

            this.articleRepository.AddAsync(article);
            this.articleRepository.SaveChangesAsync();

            return this.articleRepository.All().FirstOrDefault(p => p.Title == model.Title).Id;
            
        }

        public ArticleViewModel GetArticleById(int id)
        {
           var model = this.articleRepository.All().Where(p => p.Id == id)
                .To<ArticleViewModel>()
                .SingleOrDefault();

            model.Comments = this.commentRepository.All().Where(p => p.ArticleId == id)
                .To<CommentViewModel>()
                .ToArray();

            model.TagNames = this.articleTagRepository.All().Where(p => p.ArticleId == id)
                .Select(p => new TagIdAndNameModel
                {
                    Name = p.Tag.Name, 
                    Id = p.Tag.Id
                })
                .ToArray();

            return model; 
        }

        public void Edit(ArticleViewModel model)
        {
            var article = this.articleRepository.All().Where(p => p.Id == model.Id)
                .SingleOrDefault();

            article.Title = model.Title;
            article.Content = model.Content;

            var category = this.categoryRepository.All().Where(p => p.Name == model.CategoryName).FirstOrDefault();
            article.Category = category;

            this.articleRepository.SaveChangesAsync();           

        }

        public void Delete(int id)
        {
            var article = this.articleRepository.All().Where(p => p.Id == id).SingleOrDefault();

            this.articleRepository.Delete(article);
            this.articleRepository.SaveChangesAsync();
        }

        public bool DoesArticleNameExist(string name)
        {
            return this.articleRepository.All().Any(p => p.Title == name);
        }
    }
}
