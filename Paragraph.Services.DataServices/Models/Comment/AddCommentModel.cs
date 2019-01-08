using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Models.Comment
{
    public class AddCommentModel
    {
        [Required]
        [MinLength(3, ErrorMessage ="Your comment should be longer than 3 symbols!")]
        public string Content { get; set; }
    }
}
