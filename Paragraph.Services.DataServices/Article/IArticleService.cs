using Paragraph.Services.DataServices.Models.Home;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Paragraph.Services.DataServices
{
    using Models.Article;

    public interface IArticleService
    {
        IndexViewModel GetArticles(int num);
        IEnumerable<ArticleIdAndName> All();
        int Create(CreateArticleInputModel model, string username);
        ArticleViewModel GetArticleById(int id);
        void Edit(ArticleViewModel model);       
        void Delete(int id);
        bool DoesArticleExist(int id);
        bool DoesArticleNameExist(string title);
    }
}