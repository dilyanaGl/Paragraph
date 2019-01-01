using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Article
{
    using Mapping;
    using Data.Models;
    using Comment;

    public class ArticleViewModel : IMapFrom<Article>
    {
        private string title;

        public String Title { get => this.title.TrimStart('?'); set => this.title = value; }

        public string Content { get; set; }

        public string HtmlContent { get => this.Content == null ? null : this.Content.TrimStart('?'); }

        public string CategoryName { get; set; }

        public int Id { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public AddCommentModel AddCommentModel { get; set; }
    }
}
