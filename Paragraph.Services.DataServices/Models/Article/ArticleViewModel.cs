using System;
using System.Collections.Generic;
using System.Linq;

namespace Paragraph.Services.DataServices.Models.Article
{
    using Mapping;
    using Data.Models;
    using Comment;
    using AutoMapper;

    public class ArticleViewModel : IMapFrom<Article>, IHaveCustomMappings
    {
        private string title;

        public String Title { get => this.title.TrimStart('?'); set => this.title = value; }

        public string Content { get; set; }

        public string HtmlContent { get => this.Content == null ? null : this.Content.TrimStart('?'); }

        public string CategoryName { get; set; }

        public int Id { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public AddCommentModel AddCommentModel { get; set; }

        public IEnumerable<IdAndNameModel> TagNames { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Paragraph.Data.Models.Article, ArticleViewModel>()
            .ForMember(p => p.TagNames,
                       mapping => mapping.MapFrom(c =>
                       c.Tags.Select(p => new IdAndNameModel
                       {
                           Id = p.Tag.Id,
                           Name = p.Tag.Name
                       })
                       .ToArray()
                       ));
        }
    }
}
