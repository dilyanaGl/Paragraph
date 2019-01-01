using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Category
{
    public class ListCategoriesModel
    {
        public ICollection<CategoryIdAndNameModel> Categories { get; set; }
    }
}
