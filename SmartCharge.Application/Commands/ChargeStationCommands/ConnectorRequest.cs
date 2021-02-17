using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeStationCommands
{
     public class ConnectorRequest
    {
              
        public int ConnectorId { get; set; }
        public decimal MaxCurrentAmps { get; set; }
               
    }
}
