using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Paragraph.Services.DataServices
{
    using Data.Common;
    using Data.Models;
    using Models.Tag;
    using Models.Article;
    using Mapping;
    using AutoMapper;

    public class TagService : ITagService
    {
        private readonly IRepository<Data.Models.Tag> tagRepsitory;
        private readonly IRepository<Data.Models.Article> articleRepository;
        private readonly IRepository<ArticleTag> articleTagRepository;
       

        public TagService(IRepository<Data.Models.Tag> tagRepsitory, IRepository<Data.Models.Article> articleRepository, IRepository<ArticleTag> articleTagRepository)
        {
            this.tagRepsitory = tagRepsitory;
            this.articleRepository = articleRepository;
            this.articleTagRepository = articleTagRepository;
            
        }

        public void Create(CreateTagModel model)
        {
            var tag = new Data.Models.Tag
            {
                Name = model.Name
            };

            this.tagRepsitory.AddAsync(tag);
            this.tagRepsitory.SaveChangesAsync();
        }

        public void AddTagToArticle(int tagId, int articleId)
        {
           
            var article = this.articleRepository.All().SingleOrDefault(p => p.Id == articleId);      

           
                var tag = this.tagRepsitory.All().FirstOrDefault(p => p.Id == tagId);
                        

                var articleTag = new ArticleTag
                {
                    Tag = tag,
                    Article = article

                };         
            
             this.articleTagRepository.AddAsync(articleTag);
             this.articleTagRepository.SaveChangesAsync();

        }       

        public TagViewModel Details(int id)
        {
            var model = this.tagRepsitory.All()
                .Where(p => p.Id == id)
            .To<TagViewModel>()
            .FirstOrDefault();

            model.Articles = this.articleTagRepository.All().Where(p => p.Tag.Id == id)
                .Select(p => new ArticleIdAndName
                {
                    Title = p.Article.Title,
                    Id = p.ArticleId
                })
                .ToArray();

            return model;
        }

        public IEnumerable<TagIdAndNameModel> All()
        {
            return this.tagRepsitory.All()
                .OrderByDescending(p => p.ArticleTags.Count())
                .To<TagIdAndNameModel>()
                .ToArray();
        }

        public IEnumerable<TagIdAndNameModel> TagsForArticle(int id)
        {
            return this.tagRepsitory.All()
                .Where(p => !this.articleTagRepository.All()
                .Any(k => k.ArticleId == id && k.TagId == p.Id))
                .To<TagIdAndNameModel>()
                .ToArray();
        }

        public bool IsTagValid(int id)
        {
            return this.tagRepsitory.All().Any(p => p.Id == id);
        }

        //public bool DoesTagNameExist(string categoryName)
        //{
        //    return this.tagRepsitory.All().Any(p => p.Name == categoryName);
        //}

        public bool DoesТаgNameExist(string categoryName)
        {
            return this.tagRepsitory.All().Any(p => p.Name == categoryName);
        }
    }
}
