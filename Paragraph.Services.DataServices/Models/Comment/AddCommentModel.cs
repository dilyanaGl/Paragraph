using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Models.Comment
{
    public class AddCommentModel
    {
        [Required]
        [MinLength(3)]
        public string Content { get; set; }
    }
}
