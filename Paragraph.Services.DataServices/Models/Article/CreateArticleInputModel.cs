using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Paragraph.Services.DataServices.Models.Article
{
    using Attributes.Article;
    using Attributes;

   public class CreateArticleInputModel 
    {
        [Required]
        [MinLength(3,ErrorMessage ="Your title should be at least 3 symbols!")]
        [ValidArticleTitle]
        public string Title { get; set; }

        [Required]
        [MinLength(3, ErrorMessage ="Your content should be at least 3 symbols!")]
        public string Content { get; set; }

        [ValidCategoryId]
        public int CategoryId { get; set; }

       

        
    }
}
