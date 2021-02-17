using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
     public class AddChargeStationDto : IMapFrom<ChargeStation>
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Connector> Connectors { get; set; }
        public string ChargeGroupId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeStation, AddChargeStationDto>()
                .ForMember(destination => destination.ChargeGroupId,
               opts => opts.MapFrom(source => source.ParentChargeGroup.Id));
           
        }
    }
}
