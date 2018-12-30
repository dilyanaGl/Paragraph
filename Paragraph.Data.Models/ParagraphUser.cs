using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Paragraph.Data.Models
{
    // Add profile data for application users by adding properties to the ParagraphUser class
    public class ParagraphUser : IdentityUser
    {
        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();

        public ICollection<Comment> Comment { get; set; } = new HashSet<Comment>();
    }
}
