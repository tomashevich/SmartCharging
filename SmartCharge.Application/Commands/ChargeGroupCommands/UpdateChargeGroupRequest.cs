using Application.Common.Mappings;
using AutoMapper;
using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Application.Posts.Commands.ChargeGroupCommands
{
     public class UpdateChargeGroupRequest 
    {
      

        public string Name { get; set; }

        public decimal CapacityAmps { get; set; }

 

       
    }
}
