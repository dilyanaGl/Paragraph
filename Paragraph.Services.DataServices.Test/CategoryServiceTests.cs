using System;
using System.Collections.Generic;
using System.Linq;
using Paragraph.Services.DataServices.Models.Category;
using Xunit;
using Paragraph.Data;
using Paragraph.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Paragraph.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Paragraph.Services.DataServices.Models.Article;
using Paragraph.Services.DataServices.Models.Home;
using Paragraph.Data.Models;


namespace Paragraph.Services.DataServices.Tests
{
    public class CategoryServiceTests
    {

        private readonly ParagraphContext context;
        private readonly ICategoryService categoryService;
        private readonly IServiceProvider provider;


        public CategoryServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ParagraphContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IArticleService, ArticleService>();

            AutoMapperConfig.RegisterMappings(
            typeof(ArticleViewModel).Assembly,
            typeof(IndexArticleViewModel).Assembly
            );


            this.provider = services.BuildServiceProvider();
            this.context = provider.GetService<ParagraphContext>();
            this.categoryService = provider.GetService<ICategoryService>();
        }

        [Fact]
        public void TestIf_AddCategory_InsertsCategoryInTheDatabase()
        {
            var category = new AddCategoryModel();
            this.categoryService.AddCategory(category);

            Assert.Single(this.context.Categories);
        }

        [Fact]
        public void TestIf_DoesCategoryExist_ReturnsCorrectValue()
        {
            this.context.Categories.Add(new Category
            {
                Id = 1
            });

            this.context.SaveChanges();

            Assert.True(this.categoryService.DoesCategoryExist(1));
            Assert.False(this.categoryService.DoesCategoryExist(2));
        }

        [Fact]
        public void TestIf_GetCategories_ReturnsCorrectCount()
        {
            this.AddCategoriesToDatabase();

            Assert.Equal(3, this.categoryService.GetCategories().Count());
        }

        [Fact]
        public void TestIf_GetCategoryWithArticles_ReturnsCorrectArticle()
        {
            this.AddCategoriesToDatabase();

            Assert.Matches("1", this.categoryService.GetCategoryWithArticles(1).Name);
            Assert.Matches("2", this.categoryService.GetCategoryWithArticles(2).Name);
            Assert.Matches("3", this.categoryService.GetCategoryWithArticles(3).Name);
        }

        [Fact]
        public void TestIf_IsCategoryVald_ReturnsCorrectValue()
        {
            this.AddCategoriesToDatabase();

            Assert.True(this.categoryService.IsCategoryVald(1));
            Assert.True(this.categoryService.IsCategoryVald(2));
            Assert.True(this.categoryService.IsCategoryVald(3));

            Assert.False(this.categoryService.IsCategoryVald(11));
            Assert.False(this.categoryService.IsCategoryVald(12));
            Assert.False(this.categoryService.IsCategoryVald(13));

        }

        [Fact]
        public void TestIf_ListCategoriesAndCount_ReturnsCorrectCount()
        {
            this.AddCategoriesToDatabase();

            Assert.Equal(3, this.categoryService.ListCategoriesAndCount().Categories.Count());


        }

        [Fact]
        public void TestIf_ListCategories_ReturnsCategoryInCorrectOrder()
        {
            this.AddCategoriesToDatabase();
            this.AddArticlesToCategories();

            var expectedValues = this.context.Categories.OrderByDescending(p => p.Articles.Count).Select(p => p.Name).ToArray();

            var actualValues = this.categoryService.ListCategoriesAndCount().Categories.Select(p => p.Name).ToArray();

            Assert.Matches(expectedValues[0], actualValues[0]);
            Assert.Matches(expectedValues[1], actualValues[1]);
            Assert.Matches(expectedValues[2], actualValues[2]);

            Assert.Matches("1", actualValues[0]);
            Assert.Matches("2", actualValues[1]);
        }

        private void AddArticlesToCategories()
        {
            var articles = new List<Article>
            {
                new Article{Id = 1, Title ="cool1"},
                new Article{Id = 2, Title ="cool2"},
                new Article{Id = 3, Title ="cool3"},
                new Article{Id = 4, Title ="cool4"}
            };

            this.context.Articles.AddRange(articles);
            this.context.SaveChanges();

            var currentArticles = this.context.Articles.ToArray();
            for (int i = 0; i < currentArticles.Length; i++)
            {
                if (i == 0 || i == 1)
                {
                    this.context.Categories.FirstOrDefault(p => p.Id == 1).Articles.Add(currentArticles[i]);
                }
                else if (i == 2)
                {
                    this.context.Categories.FirstOrDefault(p => p.Id == 2).Articles.Add(currentArticles[i]);
                    this.context.SaveChanges();
                }
                else
                {
                    this.context.Categories.FirstOrDefault(p => p.Id == 3).Articles.Add(currentArticles[i]);
                    this.context.SaveChanges();
                }
            }

        }



[Fact]
public void TestIf_GetCategoryWithArticles_ReturnsCorrectNumberOfArticles()
{
    this.AddCategoriesToDatabase();
    this.AddArticlesToCategories();


    var model1 = this.categoryService.GetCategoryWithArticles(1);
    var model2 = this.categoryService.GetCategoryWithArticles(2);
    var model3 = this.categoryService.GetCategoryWithArticles(3);

    Assert.Equal(2, model1.ArticlesCount);
    Assert.Single(model2.Articles);
    Assert.Single(model3.Articles);
}

[Fact]
public void TestIf_DoesCategoryNameExist_ReturnsCorrectValue()
{
    this.AddCategoriesToDatabase();
    Assert.True(this.categoryService.DoesCategoryNameExist("1"));
    Assert.False(this.categoryService.DoesCategoryNameExist("11"));


}

// Setup
private void AddCategoriesToDatabase()
{
    this.context.Categories.AddRange(new List<Category>
            {
                new Category{Name = "1", Id = 1},
                new Category{Name = "2", Id = 2},
                new Category{Name = "3", Id = 3},
            });
    this.context.SaveChanges();
}
    }
}
