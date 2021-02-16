using SmartCharge.Core.Entities;
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
        Task AddAsync(ChargeGroup chargeGroup);
        Task UpdateAsync(ChargeGroup chargeGroup);
        Task <long> DeleteAsync(Guid chargeGroupId);
    }
}
