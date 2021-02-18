using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System.Collections.Generic;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class ChargeStationDto : IMapFrom<ChargeStation>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Connector> Connectors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeStation, ChargeStationDto>();
        }
    }
}
