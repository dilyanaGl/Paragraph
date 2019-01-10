using System;
using System.Collections.Generic;
using System.Linq;
using Paragraph.Services.DataServices.Models.Article;
using Paragraph.Services.DataServices.Models.Home;
using Xunit;
using Moq;
using Paragraph.Data.Common;
using Paragraph.Data;
using Paragraph.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Paragraph.Services.Mapping;


namespace Paragraph.Services.DataServices.Test
{
    public class ArticleServiceTests 
    {
        private readonly ParagraphContext context;
        private readonly IArticleService articleService;
        private readonly IServiceProvider provider;


        public ArticleServiceTests()
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
            this.articleService = provider.GetService<IArticleService>();
        }


        [Fact]
        public void TestIf_All_ReturnsCorrectNumber()
        {
             var articles = new List<Article>
            {
                new Article { Id = 1, Title = "1"},
                new Article { Id = 2, Title = "2"},
                new Article { Id = 3, Title = "3"}
            };

            this.context.Articles.AddRange(articles);
            this.context.SaveChanges();

            Assert.Equal(3, articleService.All().Count());            
        }

        [Fact]
        public void TestIf_Create_AddsAnArticle()
        {
            Assert.Empty(this.context.Articles);

            var article = new CreateArticleInputModel();
            this.context.Users.Add(new ParagraphUser
            {
                UserName = "test"
            });
            this.context.SaveChanges();

            this.articleService.Create(article, "test");

            Assert.Single(this.context.Articles);
            
          
        }

        [Fact]
        public void TestIf_DeletesAnArticle_RemovesSaidArticle()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "1"},
                new Article { Id = 2, Title = "2"},
                new Article { Id = 3, Title = "3"}
            };

            this.context.Articles.AddRange(articles);
            this.context.SaveChanges();         

            articleService.Delete(2);

            Assert.Equal(2, this.context.Articles.Count());
        }

        [Fact]
        public void TestIf_DoesArticleExist_ReturnsACorrectValue()
        {
            var articleRepository = new Mock<IRepository<Article>>();
            articleRepository.Setup(p => p.All()).Returns(new List<Article>
            {
                new Article{ Id= 2}
            }
            .AsQueryable());

            var articleService = new ArticleService(articleRepository.Object, null, null, null, null, null);


           Assert.True(articleService.DoesArticleExist(2));
        }

        [Fact]
        public void TestIf_DoesArticleExist_ReturnsFalse()
        {
            var articleRepository = new Mock<IRepository<Article>>();
            articleRepository.Setup(p => p.All()).Returns(new List<Article>
            {
                new Article{ Id= 2}
            }
            .AsQueryable());

            var articleService = new ArticleService(articleRepository.Object, null, null, null, null, null);


            Assert.False(articleService.DoesArticleExist(1));
        }

        [Fact]
        public void TestIf_EditArticle_IsUpdatedAfterEdit()
        {
            var article = new Article { Title = "Example Title 1", Id = 1 };
            this.context.Articles.Add(article);
            this.context.SaveChanges();

            var model = new ArticleViewModel
            {
                Title = "Example Title 2", 
                Id = 1
            };

            articleService.Edit(model);

            Assert.Matches("Example Title 2", this.context.Articles.FirstOrDefault(p => p.Id == 1).Title);


        }

        [Fact]
        public void TestIf_GetArticleById_ReturnsCorrectArticle()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "1", Category= new Category{ Id = 1, Name = "2"}},
                new Article { Id = 2, Title = "2", Category= new Category{ Id = 3, Name = "2"}},
                new Article { Id = 3, Title = "3", Category= new Category{ Id = 2, Name = "2"}}
            };

            this.context.Articles.AddRange(articles);
            this.context.SaveChanges();
           

            Assert.Matches("1", articleService.GetArticleById(1).Title);
            Assert.Matches("2", articleService.GetArticleById(2).Title);
            Assert.Matches("3", articleService.GetArticleById(3).Title);

            

            Assert.NotEqual("2", articleService.GetArticleById(3).Title);
            Assert.NotEqual(2, articleService.GetArticleById(3).Id);

        }

        [Fact]
        public void ТestIf_GetArticles_ReturnsACorrentNumberOfArticles()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "1", Content="1", Category= new Category{ Id = 1, Name = "2"} },
                new Article { Id = 2, Title = "2", Content="1", Category= new Category{ Id = 2, Name = "2"}},
                new Article { Id = 3, Title = "3", Content="1", Category= new Category{ Id = 3, Name = "2"}}
            };

            this.context.Articles.AddRange(articles);
            this.context.SaveChanges();


             Assert.Equal(2, articleService.GetArticles(2).Articles.Count());
            Assert.Equal(3, articleService.GetArticles(3).Articles.Count());
        }

        [Fact]
        public void TestIf_DoesTitleExist_ReturnsCorrectResult()
        {
            var articleRepository = new Mock<IRepository<Article>>();
            articleRepository.Setup(p => p.All()).Returns(new List<Article>
            {
                new Article{ Id= 2, Title = "New Title"}
            }
            .AsQueryable());

            var articleService = new ArticleService(articleRepository.Object, null, null, null, null, null);


            Assert.True(articleService.DoesArticleNameExist("New Title"));
            Assert.False(articleService.DoesArticleNameExist("New Title 2"));
        }
    }
}
