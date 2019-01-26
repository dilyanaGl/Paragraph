using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Models.Tag
{
    using Attributes;

    public class AddTagModel
    {
        [ValidTagId]              
        public int TagId { get; set; }
           
        public string Name { get; set; }
    }
}
