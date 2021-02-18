using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Application.Commands.ChargeStationCommands;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
    public class AddChargeGroupDto : IMapFrom<ChargeGroup>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CapacityAmps { get; set; }
        public IEnumerable<ChargeStationDto> ChargeStations { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeGroup, AddChargeGroupDto>();
        }
    }
}
