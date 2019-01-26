using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Request
{
    using Services.Mapping;
    using Data.Models;
    using AutoMapper;

    public class RequestModel : IMapFrom<Request>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Request, RequestModel>()
                .ForMember(p => p.Role,
                           mapping => mapping.MapFrom(c => c.Role.Name));
        }
    }


}
