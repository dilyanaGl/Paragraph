
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Paragraph.Services.DataServices.Models.Category
{
    using AutoMapper;
    using Mapping;

    public class CategoryIdAndNameModel : IdAndNameModel, IMapFrom<Paragraph.Data.Models.Category>, IHaveCustomMappings
    {
        public int CountOfAllArticles { get; set; }


        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Paragraph.Data.Models.Category, CategoryIdAndNameModel>()
                .ForMember(p => p.CountOfAllArticles,
                           mapping => mapping.MapFrom(c => c.Articles.Count()));
        }
    }
}
