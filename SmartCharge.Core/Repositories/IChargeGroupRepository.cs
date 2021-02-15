﻿using SmartCharge.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharge.Core.Repositories
{
   
     public interface IChargeGroupRepository
    {
        Task<ChargeGroup> GetAsync(Guid chargeGroupId);
        Task<bool> ExistsAsync(Guid chargeGroupId);
        void Add(ChargeGroup chargeGroup);
        Task UpdateAsync(ChargeGroup chargeGroup);
        Task DeleteAsync(Guid chargeGroupId);
    }
}