using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Data.Models
{
    using Common;

    public class Comment : BaseModel<int>
    {
        public int AuthorId { get; set; }

        public ParagraphUser Author { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public string Content { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
    }
}
