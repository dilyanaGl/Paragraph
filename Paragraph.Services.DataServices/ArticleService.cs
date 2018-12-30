using System;
using System.Collections.Generic;
using System.Linq;


namespace Paragraph.Services.DataServices
{
    using Data.Models;
    using Paragraph.Data.Common;
    using Models.Home;
    using Models.Article;
    using System.Threading.Tasks;
    using Paragraph.Services.Mapping;
    

    public class ArticleService : IArticleService
    {
        private readonly IRepository<Data.Models.Article> articleRepository;
        private readonly IRepository<Data.Models.Category> categoryRepository;
        private readonly IRepository<Data.Models.ParagraphUser> userRepository;

        public ArticleService(IRepository<Paragraph.Data.Models.Article> articleRepository, IRepository<Data.Models.Category> categoryRepository, IRepository<ParagraphUser> userRepository)
        {
            this.articleRepository = articleRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
        }

        public IndexViewModel GetArticles(int num)
        {
            var articles =  articleRepository.All().Take(num)
                .To<IndexArticleViewModel>()

                .ToArray();

            var model = new IndexViewModel { Articles = articles };

            return model;
        }

        public async Task<int> Create(CreateArticleInputModel model, string username)
        {
            var category = this.categoryRepository.All().SingleOrDefault(p => p.Id == model.CategoryId);
            var author = this.userRepository.All().FirstOrDefault(p => p.UserName == username);

            if(category == null || author == null)
            {

            }
            
            var article = new Data.Models.Article
            {
                Title = model.Title,
                Content = model.Content,
                CategoryId = model.CategoryId,
                Author = author               

            };

            await this.articleRepository.AddAsync(article);
            await this.articleRepository.SaveChangesAsync();

            return article.Id;
        }

        public ArticleViewModel GetArticleById(int id)
        {
            return this.articleRepository.All().Where(p => p.Id == id)
                .To<ArticleViewModel>()
                //.Select(p => new ArticleViewModel
                //{
                //    Title = p.Title,
                //    Content = p.Content, 
                //    Category = p.Category.Name
                //})
                .SingleOrDefault();
        }



        public Task<int> Edit(ArticleViewModel model)
        {
            var article = this.articleRepository.All().Where(p => p.Id == model.Id)
                .SingleOrDefault();

            article.Title = model.Title;
            article.Content = model.Content;

            var category = this.categoryRepository.All().Where(p => p.Name == model.CategoryName).FirstOrDefault();
            article.Category = category;

            return this.articleRepository.SaveChangesAsync();
            

        }

        public async Task<int> Delete(int id)
        {
            var article = this.articleRepository.All().Where(p => p.Id == id).SingleOrDefault();

            this.articleRepository.Delete(article);
            return await this.articleRepository.SaveChangesAsync();
        }
     

       
    }
}
