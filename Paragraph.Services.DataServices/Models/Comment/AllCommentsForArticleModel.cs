using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Comment
{
    public class AllCommentsForArticleModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
