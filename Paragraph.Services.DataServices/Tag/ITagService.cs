using System.Threading.Tasks;
using Paragraph.Services.DataServices.Models.Tag;
using System.Collections.Generic;

namespace Paragraph.Services.DataServices
{
    public interface ITagService
    {
        void AddTagToArticle(int tagId, int articleId);
        void Create(CreateTagModel model);
        TagViewModel Details(int id);
        IEnumerable<TagIdAndNameModel> All();
        bool IsTagValid(int id);
        bool DoesТаgNameExist(string categoryName);
        IEnumerable<TagIdAndNameModel> TagsForArticle(int id);


    }
}