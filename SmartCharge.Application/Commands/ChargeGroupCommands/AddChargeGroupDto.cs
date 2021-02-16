using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
     public class AddChargeGroupDto : IMapFrom<ChargeGroup>
    {
        //private ISet<ChargeStation> _chargeStations = new HashSet<ChargeStation>();

        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal CapacityAmps { get; set; }

        // public decimal CapacityReserve { get; private set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeGroup, AddChargeGroupDto>()
                ;
        }
    }
}
