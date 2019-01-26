using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.User
{
    using Article;
    using Request;
    

    public class UserDetailsModel 
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public ICollection<ArticleIdAndName> Articles { get; set; }

        public IEnumerable<ListRequestModel> RequestModels { get; set; }

        public RequestModel MakeRequestModel { get; set; }


    }
}
