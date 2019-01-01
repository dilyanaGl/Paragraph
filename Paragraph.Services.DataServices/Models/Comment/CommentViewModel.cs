using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Paragraph.Services.DataServices.Models.Comment
{
    using Services.Mapping;
    using Data.Models;

    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string AuthorName { get; set; }

        public int Id { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Paragraph.Data.Models.Comment, CommentViewModel>()
            .ForMember(p => p.AuthorName,
                       mapping => mapping.MapFrom(c =>
                       c.Author.UserName
                       ));

            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(p => p.PublishedOn,
                mapping => mapping.MapFrom(c => c.PublishedOn.ToString("dd/MM/yyyy")));
        }
    }
}
