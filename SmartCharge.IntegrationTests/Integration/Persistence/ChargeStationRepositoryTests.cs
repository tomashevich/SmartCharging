using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SmartCharge.Core.Entities;
using SmartCharge.Infrastructure.Mongo.Repositories;
using Xunit;

namespace SmartCharging.IntegrationTests.Integration.Persistence
{
    public class ChargeStationRepositoryTests : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        public ChargeStationRepositoryTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ExistsAsync_should_return_false_when_no_data()
        {
            var stations = new ChargeStationRepository(_fixture.DbContext);
            var result = await stations.ExistsAsync(Guid.NewGuid()).ConfigureAwait(false);
            result.ShouldBeFalse();
        }


        [Fact]
        public async Task AddAsync_should_add_chargeStation()
        {
            var stations = new ChargeStationRepository(_fixture.DbContext);
            var capacity = 10m;
            var name = "TestChargeStation";
            var chargeStationId = Guid.NewGuid();

            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(chargeStationId, name, chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            //add
            await stations.AddAsync(chargeStation).ConfigureAwait(false);

            //get
            var stationFromDb = await stations.GetAsync(chargeStationId).ConfigureAwait(false);
            stationFromDb.Id.ShouldBe(chargeStationId);
            stationFromDb.Name.ShouldBe(name);
            stationFromDb.ParentChargeGroup.ShouldBeNull();
        }

        [Fact]
        public async Task UpdateConnectors_should_update_chargestation_connectors()
        {
            var stations = new ChargeStationRepository(_fixture.DbContext);
            var capacity = 10m;
            var name = "TestChargeStation";
            var chargeStationId = Guid.NewGuid();

            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(chargeStationId, name, chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);

            //add
            await stations.AddAsync(chargeStation).ConfigureAwait(false);

            //get
            var stationFromDb = await stations.GetAsync(chargeStationId).ConfigureAwait(false);
            stationFromDb.Id.ShouldBe(chargeStationId);
            stationFromDb.Connectors.Count().ShouldBe(0);

            chargeStation.AddConnector(3.0m);
            await stations.UpdateConnectorsAsync(chargeStation).ConfigureAwait(false);
            stationFromDb = await stations.GetAsync(chargeStationId).ConfigureAwait(false);
            stationFromDb.Id.ShouldBe(chargeStationId);
            stationFromDb.Connectors.Count().ShouldBe(1);
            stationFromDb.Connectors.FirstOrDefault().MaxCurrentAmps.ShouldBe(3.0m);
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

            var stationFromDb = await stations.GetAsyncExtended(stationId).ConfigureAwait(false);
            stationFromDb.Id.ShouldBe(stationId);
            stationFromDb.ParentChargeGroup.Id.ShouldBe(groupId);
            stationFromDb.ParentChargeGroup.ChargeStations.First(s => s.Id == stationId).ShouldNotBeNull();
        }
    }
}
