using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.DataServices.Models.Request
{
    using Services.Mapping;
    using Data.Models;
    using AutoMapper;


    public class ListRequestModel : RequestModel, IMapFrom<Request>
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            base.CreateMappings(configuration);

            configuration.CreateMap<Request, ListRequestModel>()
                .ForMember(p => p.Status,
                           mapping => mapping.MapFrom(c => c.Status.ToString()));

            
        }
    }
}
