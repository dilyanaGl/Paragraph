using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Paragraph.Services.DataServices.Models.Category
{
    using Services.Mapping;
    using Data.Models;
    using AutoMapper;
    using Article;


    public class CategoryWithArticlesModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public string Name { get; set; }

        
        public int ArticlesCount { get; set; }

        public IEnumerable<ArticleIdAndName> Articles { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Paragraph.Data.Models.Category, CategoryWithArticlesModel>()
            .ForMember(p => p.Articles,
                       mapping => mapping.MapFrom(c =>
                       c.Articles.Select(p => new ArticleIdAndName
                       {
                           Id = p.Id, 
                           Title = p.Title
                       })
                       .ToArray()
                       ));
        }
    }

   
}
