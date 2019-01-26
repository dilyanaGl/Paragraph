using System;
using System.Collections.Generic;
using System.Linq;

namespace Paragraph.Services.DataServices.Models.Tag
{
    using Mapping;
    using Data.Models;
    using Article;
    using AutoMapper;
    

    public class TagViewModel : IMapFrom<Tag>
    {
        public string Name { get; set; }

        public IEnumerable<ArticleIdAndName> Articles { get; set; }
   

    public void CreateMappings(IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<Paragraph.Data.Models.Tag, TagViewModel>()
        .ForMember(p => p.Articles,
                   mapping => mapping.MapFrom(c =>
                   c.ArticleTags.Select(p => new ArticleIdAndName
                   {
                       Id = p.Article.Id,
                      Title = p.Article.Title
                   })
                   .ToArray()
                   ));

        }
    }
}
