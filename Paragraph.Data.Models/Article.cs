using System;
using System.Collections.Generic;

namespace Paragraph.Data.Models
{
    using Data.Common;

    public class Article : BaseModel<int>
    {

        public string Title { get; set; }

        public string Content { get; set; }

        public string Picture { get; set; }

        public string AuthorId { get; set; }

        public ParagraphUser Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public bool IsApproved { get; set; }

       public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<ArticleTag> Tags { get; set; } = new HashSet<ArticleTag>();
    }
}
