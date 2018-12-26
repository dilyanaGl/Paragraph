using Paragraph.Services.DataServices.Models.Category;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices
{
    public interface ICategoryService
    {
        IEnumerable<IdAndNameModel> GetCategories();
        bool IsCategoryVald(int categoryId);
    }
}