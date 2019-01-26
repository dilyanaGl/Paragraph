using Paragraph.Services.DataServices.Models.Category;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Paragraph.Services.DataServices
{
    public interface ICategoryService
    {
        IEnumerable<IdAndNameModel> GetCategories();
        bool IsCategoryVald(int categoryId);
        bool DoesCategoryNameExist(string categoryName);
        ListCategoriesModel ListCategoriesAndCount();
        CategoryWithArticlesModel GetCategoryWithArticles(int id);
        void AddCategory(AddCategoryModel model);
        bool DoesCategoryExist(int id);

    }
}