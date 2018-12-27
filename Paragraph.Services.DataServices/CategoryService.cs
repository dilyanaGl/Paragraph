using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Paragraph.Services.DataServices
{
    using Data.Models;
    using Data.Common;
    using Data;
    using Models.Category;
    using System.Linq;
    using Paragraph.Services.Mapping;

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Paragraph.Data.Models.Category> categoryRepository;
              

        public CategoryService(IRepository<Paragraph.Data.Models.Category> repository)
        {
            this.categoryRepository = repository;
        }

        public IEnumerable<IdAndNameModel> GetCategories()
        {
            var categories = this.categoryRepository
                .All()
                .To<IdAndNameModel>()
                 //.Select(p => new IdAndNameModel
                //{
                //    Name = p.Name,
                //    Id = p.Id
                //})
                .ToArray();         

            return categories;
        }

        public bool IsCategoryVald(int categoryId)
        {
            return this.categoryRepository.All().Any(p => p.Id == categoryId);
        }

        public ListCategoriesModel ListCategoriesAndCount()
        {
            var categories = this.categoryRepository.All()
                .OrderBy(p => p.Name)
                .To<CategoryIdAndNameModel>().ToArray();

            var model = new ListCategoriesModel
            {
                Categories = categories
            };

            return model;
        }

        public CategoryWithArticlesModel GetCategoryWithArticles(int id)
        {
            var model = this.categoryRepository.All().Where(p => p.Id == id)
                .To<CategoryWithArticlesModel>()
                .SingleOrDefault();

            return model;

        }
    }
}
