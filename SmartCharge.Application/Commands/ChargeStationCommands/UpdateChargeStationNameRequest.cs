﻿using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
     public class UpdateChargeStationNameRequest
    {
      

        public string Name { get; set; }

       
    }
}