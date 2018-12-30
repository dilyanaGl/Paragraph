using Paragraph.Services.DataServices.Models.Home;
using System.Threading.Tasks;


namespace Paragraph.Services.DataServices
{
    using Models.Article;

    public interface IArticleService
    {
        IndexViewModel GetArticles(int num);
        Task<int> Create(CreateArticleInputModel model, string username);
        ArticleViewModel GetArticleById(int id);
        Task<int> Edit(ArticleViewModel model);
       
        Task<int> Delete(int id);
    }
}