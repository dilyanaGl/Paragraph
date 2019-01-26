using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Tag
{
    using Services.Mapping;
    using Data.Models;

    public class TagIdAndNameModel : IdAndNameModel, IMapFrom<Tag>
    {
    }
}
