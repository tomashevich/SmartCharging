using SmartCharge.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SmartCharge.Core.Repositories
{
    public interface IChargeStationRepository
    {
        Task<ChargeStation> GetAsync(Guid chargeStationId);
        Task<ChargeStation> GetAsyncExtended(Guid chargeStationId);
        Task<bool> ExistsAsync(Guid chargeStationId);
        Task AddAsync(ChargeStation chargeStation);
        Task UpdateNameAsync(Guid id, string name);
        Task UpdateChargeGroupAsync(Guid id, Guid chargeGroupId);
        Task UpdateConnectorsAsync(ChargeStation chargeStation);
        Task<long> DeleteAsync(Guid chargeStationId);
    }
}
