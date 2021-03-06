﻿using SmartCharge.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SmartCharge.Core.Repositories
{
    public interface IChargeGroupRepository
    {
        Task<ChargeGroup> GetAsync(Guid chargeGroupId);
        Task<ChargeGroup> GetAsyncExtended(Guid chargeGroupId);
        Task<bool> ExistsAsync(Guid chargeGroupId);
        Task AddAsync(ChargeGroup chargeGroup);
        Task UpdateAsync(ChargeGroup chargeGroup);
        Task <long> DeleteAsync(Guid chargeGroupId);
    }
}
