using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
     public class ChargeStationDto : IMapFrom<ChargeStation>
    {
        //private ISet<ChargeStation> _chargeStations = new HashSet<ChargeStation>();

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Connector> Connectors { get; set; }
      

      

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeStation, ChargeStationDto>();
                
           
        }
    }
}
