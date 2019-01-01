using System.Collections.Generic;
using Paragraph.Services.DataServices.Models.Comment;

namespace Paragraph.Services.DataServices
{
    public interface ICommentService
    {
        void AddComment(AddCommentModel model, string username, int articleId);
        IEnumerable<CommentViewModel> DisplayComments(int articleId);
        void Delete(int id);
    }
}