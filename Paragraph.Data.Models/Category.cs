using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Data.Models
{
    using Data.Common;

    public class Category : BaseModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}
