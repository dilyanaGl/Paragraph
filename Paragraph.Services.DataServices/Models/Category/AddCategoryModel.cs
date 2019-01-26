using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Paragraph.Services.DataServices.Models.Category
{
    using DataServices.Attributes;

    public class AddCategoryModel
    {
        [Required]
        [MinLength(3, ErrorMessage ="Your category should be at least 3 symbols!")]
        [ValidCategoryName]
        public string Name { get; set; }
    }
}
