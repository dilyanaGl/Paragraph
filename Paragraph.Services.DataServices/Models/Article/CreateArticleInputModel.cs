using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Paragraph.Services.DataServices.Models.Article
{
    using Attributes;

   public class CreateArticleInputModel 
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(20)]
        public string Content { get; set; }

        [ValidCategoryId]
        public int CategoryId { get; set; }

        public string Tags { get; set; }
    }
}
