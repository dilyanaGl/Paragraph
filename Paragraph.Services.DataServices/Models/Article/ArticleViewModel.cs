using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Article
{
    using Mapping;
    using Data.Models;

    public class ArticleViewModel : IMapFrom<Article>
    {
        public String Title { get; set; }

        public string Content { get; set; }

        public string HtmlContent { get => this.Content.TrimStart('?').Replace("\n", "<br />\n"); }

        public string CategoryName { get; set; }
    }
}
