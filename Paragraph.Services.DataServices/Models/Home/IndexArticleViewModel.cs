﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Paragraph.Services.DataServices.Models.Home
{
    using Mapping;
    using Data.Models;
    
    public class IndexArticleViewModel : IMapFrom<Article>
    {
        private string content;
        private string title;

        public string Title { get => this.title.TrimStart('?'); set => this.title = value; }

        public string Content { get => this.content.TrimStart('?'); set => this.content = value; }

        public string HtmlContent { get => String.Concat(String.Join("", this.Content.Replace("\n", "<br />\n").Take(700).ToArray()), "..."); }

        public string CategoryName { get; set; }

        public int Id { get; set; }
    }
}
