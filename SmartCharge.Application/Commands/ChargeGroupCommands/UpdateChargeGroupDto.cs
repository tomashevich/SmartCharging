﻿using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Application.Commands.ChargeStationCommands;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
    public class UpdateChargeGroupDto : IMapFrom<ChargeGroup>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CapacityAmps { get; set; }
        public IEnumerable<ChargeStationDto> ChargeStations { get; set; }

        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ConnectorsToUnplug { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChargeGroup, UpdateChargeGroupDto>();
        }
    }
}
