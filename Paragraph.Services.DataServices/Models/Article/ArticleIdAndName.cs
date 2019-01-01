using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Article
{
    using Services.Mapping;
    using Data.Models;

    public class ArticleIdAndName : IMapFrom<Article> 
    {

        public int Id { get; set; }

        public string Title { get; set; }
    }
}
