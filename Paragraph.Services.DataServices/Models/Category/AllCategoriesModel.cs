using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Category
{
    public class AllCategoriesModel
    {
        public IEnumerable<IdAndNameModel> Categories { get; set; }
    }
}

