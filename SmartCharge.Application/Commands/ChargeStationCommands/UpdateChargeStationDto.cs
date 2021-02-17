﻿using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
     public class UpdateChargeStationDto : IMapFrom<ChargeStation>
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Connector> Connectors { get; set; }
        public string ChargeGroupId { get; set; }

        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        public List<string> ConnectorsToUnplug { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeStation, UpdateChargeStationDto>()
                .ForMember(destination => destination.ChargeGroupId,
               opts => opts.MapFrom(source => source.ParentChargeGroup.Id));
           
        }
    }
}
