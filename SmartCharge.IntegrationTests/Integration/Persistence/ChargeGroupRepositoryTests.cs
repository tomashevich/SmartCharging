using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SmartCharge.Core.Entities;
using SmartCharge.Infrastructure.Mongo.Repositories;
using Xunit;

namespace SmartCharging.IntegrationTests.Integration.Persistence
{
    public class ChargeGroupRepositoryTests : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        public ChargeGroupRepositoryTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ExistsAsync_should_return_false_when_no_data()
        {
            var groups = new ChargeGroupRepository(_fixture.DbContext);
            var result = await groups.ExistsAsync(Guid.NewGuid()).ConfigureAwait(false);
            result.ShouldBeFalse();
        }

        [Fact]
        public async Task AddAsync_should_add_chargeGroup()
        {
            var groups = new ChargeGroupRepository(_fixture.DbContext);
            var capacity = 10m;
            var name = "TestChargeGroup";
            var chargeGroupId = Guid.NewGuid();

            var chargeGroup = new ChargeGroup(chargeGroupId, name, capacity, null);
            //add
            await groups.AddAsync(chargeGroup).ConfigureAwait(false);
            var exists = await groups.ExistsAsync(chargeGroupId).ConfigureAwait(false);
            exists.ShouldBeTrue();
            //get
            var groupFromDb = await groups.GetAsync(chargeGroupId).ConfigureAwait(false);
            groupFromDb.Id.ShouldBe(chargeGroupId);
            groupFromDb.Name.ShouldBe(name);
            groupFromDb.CapacityAmps.ShouldBe(capacity);
        }

        [Fact]
        public async Task Update_should_update_chargeGroup_properties()
        {
            var groups = new ChargeGroupRepository(_fixture.DbContext);
            var capacity = 10m;
            var name = "TestChargeGroup";
            var chargeGroupId = Guid.NewGuid();

            var chargeGroup = new ChargeGroup(chargeGroupId, name, capacity, null);
            //add
            await groups.AddAsync(chargeGroup).ConfigureAwait(false);
            var exists = await groups.ExistsAsync(chargeGroupId).ConfigureAwait(false);
            exists.ShouldBeTrue();
            //get
            var groupFromDb = await groups.GetAsync(chargeGroupId).ConfigureAwait(false);
            groupFromDb.Id.ShouldBe(chargeGroupId);
            groupFromDb.Name.ShouldBe(name);
            groupFromDb.CapacityAmps.ShouldBe(capacity);

            capacity = 11m;
            name = "TestChargeGroup_new";
            chargeGroup.Update(name, capacity);
            //update
            await groups.UpdateAsync(chargeGroup).ConfigureAwait(false);
            groupFromDb = await groups.GetAsync(chargeGroupId).ConfigureAwait(false);
            groupFromDb.Id.ShouldBe(chargeGroupId);
            groupFromDb.Name.ShouldBe(name);
            groupFromDb.CapacityAmps.ShouldBe(capacity);
        }

        [Fact]
        public async Task AsyncExtended_should_return_station_with_linked_properties()
        {
            var stations = new ChargeStationRepository(_fixture.DbContext);
            var groups = new ChargeGroupRepository(_fixture.DbContext);

            var capacity = 10m;
            var name = "TestChargeStation";
            var stationId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var chargeGroup = new ChargeGroup(groupId, "", capacity, null);
            var chargeStation = new ChargeStation(stationId, name, chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);

            //add
            await groups.AddAsync(chargeGroup).ConfigureAwait(false);
            await stations.AddAsync(chargeStation).ConfigureAwait(false);

            var groupromDb = await groups.GetAsyncExtended(groupId).ConfigureAwait(false);
            groupromDb.Id.ShouldBe(groupId);
            groupromDb.ChargeStations.ShouldNotBeNull();
            groupromDb.ChargeStations.First(s => s.Id == stationId).ShouldNotBeNull();
            groupromDb.ChargeStations.First(s => s.Id == stationId).ParentChargeGroup.Id.ShouldBe(groupId);
        }
    }
}
