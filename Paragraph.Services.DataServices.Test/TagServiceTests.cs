using System;
using System.Collections.Generic;
using System.Text;
using Paragraph.Services.DataServices.Models.Tag;
using Xunit;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Paragraph.Services.DataServices.Tests
{
    using Data.Common;
    using Data.Models;
    using Services.Mapping;
    using DataServices.Models.Article;
    using DataServices.Models.Home;
    using AutoMapper;
    using Data;
    using Microsoft.Extensions.DependencyInjection;

    public class TagServiceTests 
    {
        private readonly ParagraphContext context;
        private readonly ITagService tagService;
        private readonly IServiceProvider provider;
        

        public TagServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ParagraphContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IArticleService, ArticleService>();

            AutoMapperConfig.RegisterMappings(
            typeof(ArticleViewModel).Assembly,
            typeof(IndexArticleViewModel).Assembly
            );


            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ParagraphContext>();
            this.tagService = provider.GetService<ITagService>();
        }


        [Fact]
        public void TestIf_AddsTagToArticle_AddsArticleCorrectly()
        {
            var articles = new List<Article>()
            {
                new Article{Id = 1 },
                new Article{Id = 2 },
                new Article{ Id = 3}
            };

            this.context.AddRange(articles);

            var tags = new List<Tag>
            {
                new Tag{Id = 1 },
                new Tag{Id = 2 },
                new Tag{ Id = 3}
            };
            this.context.Tags.AddRange(tags);

            this.context.SaveChanges();

            this.tagService.AddTagToArticle(1, 1);
            this.tagService.AddTagToArticle(2, 1);
            this.tagService.AddTagToArticle(3, 1);
            this.tagService.AddTagToArticle(2, 2);
            this.tagService.AddTagToArticle(3, 2);

            this.context.SaveChanges();

            Assert.Equal(2, this.context.Articles.FirstOrDefault(p => p.Id == 2).Tags.Count());
            Assert.Equal(3, this.context.Articles.FirstOrDefault(p => p.Id == 1).Tags.Count());
            Assert.Equal(2, this.context.Tags.FirstOrDefault(p => p.Id == 3).ArticleTags.Count());
            Assert.Equal(2, this.context.Articles.FirstOrDefault(p => p.Id == 2).Tags.Count());
            Assert.Empty(this.context.Articles.FirstOrDefault(p => p.Id == 3).Tags);
            Assert.Equal(5, this.context.ArticleTags.Count());


        }

        [Fact]
        public void TestIf_All_ReturnsCorrectNumberOfTags()
        {
            this.context.Tags.AddRange(new List<Tag>
            {
                new Tag(),
                new Tag(),
                new Tag()
            });

            this.context.SaveChanges();

            Assert.NotEmpty(context.Tags);
            Assert.Equal(3, tagService.All().Count());
            
        }

        [Fact]
        public void TestIf_EmptyDatabase_ReturnsEmptyList()
        {
           Assert.Empty(this.context.Tags);
        }

        [Fact]
        public void TestIf_Create_AddsTagToTheDatabase()
        {
            this.tagService.Create(new CreateTagModel { Name = "1" });

            Assert.Single(context.Tags);
            Assert.Matches("1", context.Tags.FirstOrDefault()?.Name);
     
        }

        [Fact]
        public void TestIf_Details_ReturnsCorrectArticle()
        {
            var tags = new List<Tag>
            {
                new Tag {Id = 11, Name="1" },
                new Tag {Id = 13, Name="3" },
                new Tag {Id = 12, Name="2"}
            };

            this.context.Tags.AddRange(tags);
            this.context.SaveChanges();

            var model = tagService.Details(11);
            Assert.IsType<TagViewModel>(model);
            Assert.Matches("1", model.Name);

            var model2 = tagService.Details(12);
            Assert.Matches("2", model2.Name);

            var model3 = tagService.Details(13);
            Assert.Matches("3", model3.Name);
        }
        
        [Fact]
        public void TestIf_IsTagValid_ReturnsCorrectValue()
        {
            var tagRepository = new Mock<IRepository<Tag>>();
            tagRepository.Setup(p => p.All()).Returns(new List<Tag>
            {
                new Tag {Id = 1 },
                new Tag {Id = 3 },
                new Tag {Id = 2}
            }
            .AsQueryable());

            var tagService = new TagService(tagRepository.Object, null, null);

            Assert.True(tagService.IsTagValid(1));
            Assert.True(tagService.IsTagValid(2));
            Assert.True(tagService.IsTagValid(3));

            Assert.False(tagService.IsTagValid(12));
            Assert.False(tagService.IsTagValid(21));
            Assert.False(tagService.IsTagValid(31));
        }

        [Fact]
        public void TestIf_TagsForArticle_ReturnCorrectTagCount()
        {

            this.context.Articles.Add(new Article { Id = 1 });
            this.context.SaveChanges();

            var tag = new Tag { Id = 2};

            this.context.Tags.Add(tag);
            this.context.SaveChanges();

            Assert.Single(tagService.TagsForArticle(1));

            var newTag = new ArticleTag { TagId = 2, ArticleId = 1 };
            this.context.ArticleTags.Add(newTag);           

            this.context.SaveChanges();

            Assert.Empty(tagService.TagsForArticle(1));


        }

        [Fact]
        public void TestIf_DoesTagNameExist_ReturnsCorrectValue()
        {
            this.context.Tags.Add(new Tag { Name = "1" });
            this.context.SaveChanges();

            Assert.True(this.tagService.DoesТаgNameExist("1"));
            Assert.False(this.tagService.DoesТаgNameExist("11"));
        }
    }
}
