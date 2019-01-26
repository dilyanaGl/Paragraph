using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Models.Tag
{
    using Mapping;
    using Data.Models;
    using Attributes;

    public class CreateTagModel : IMapTo<Tag>
    {
        [Required]
        [MinLength(3, ErrorMessage = "Your tag should be at least 3 symbols!")]
        [ValidTagName]
        public string Name { get; set; }
    }
}
