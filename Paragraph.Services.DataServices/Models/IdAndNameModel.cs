using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices
{
    using Data.Models;
    using Services.Mapping;

    public class IdAndNameModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public int Id { get; set; }
    }
}
