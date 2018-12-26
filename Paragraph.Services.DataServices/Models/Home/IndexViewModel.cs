using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paragraph.Services.DataServices.Models.Home
{
    public class IndexViewModel
    {
        public ICollection<IndexArticleViewModel> Articles { get; set; }
    }
}
