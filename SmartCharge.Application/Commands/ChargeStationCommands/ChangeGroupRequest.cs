using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
     public class ChangeGroupRequest
    {
              
        public Guid ChargeGroupId { get; set; }
               
    }
}
