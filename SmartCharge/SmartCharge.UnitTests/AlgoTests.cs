using Shouldly;
using SmartCharge.Core;
using SmartCharge.Core.Entities;
using System;
using System.Linq;
using Xunit;

namespace SmartCharge.UnitTests
{
    public class AlgoTests
    {
        [Fact]
        public void Algo_should_return_connector_with_min_current()
        {
            //Arrange
            decimal capacity = 10;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            var expectedConnectorId = 9;
            chargeStation.AddConnector(1, expectedConnectorId);
            chargeStation.AddConnector(1.5m);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(3);
            var needToFree = 1.0m;
            //Act
            var result = new Algo().FindOptions(chargeGroup, needToFree);
            //Assert
            result.Count.ShouldBe(1);
            result.FirstOrDefault().Count.ShouldBe(1);
            result.FirstOrDefault().FirstOrDefault().ConnectorId.ShouldBe(expectedConnectorId);
            result.FirstOrDefault().FirstOrDefault().Amps.ShouldBe(1);
            result.FirstOrDefault().FirstOrDefault().StationId.ShouldBe(chargeStation.Id);
        }

        [Fact]
        public void Algo_should_return_multiple_single_options_with_min_current()
        {
            //Arrange
            decimal capacity = 10;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            var expectedConnectorId = 9;
            chargeStation.AddConnector(1, expectedConnectorId);
            chargeStation.AddConnector(1);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(3);
            var needToFree = 0.9m;
            //Act
            var result = new Algo().FindOptions(chargeGroup, needToFree);
            //Assert
            result.Count.ShouldBe(2);
            result[0].Count.ShouldBe(1);
            result[1].Count.ShouldBe(1);
            result.Find(c => c[0].ConnectorId == expectedConnectorId)[0].Amps.ShouldBe(1);
            result.Find(c => c[0].ConnectorId != expectedConnectorId)[0].Amps.ShouldBe(1);
        }

        [Fact]
        public void Algo_should_return_double_options_with_min_current()
        {
            //Arrange
            decimal capacity = 10;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            var expectedConnectorId_1 = 9;
            var expectedConnectorId_2 = 8;
            chargeStation.AddConnector(0.4m, expectedConnectorId_1);
            chargeStation.AddConnector(0.5m, expectedConnectorId_2);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(3);
            var needToFree = 0.9m;
            //Act
            var result = new Algo().FindOptions(chargeGroup, needToFree);
            //Assert
            result.Count.ShouldBe(1);
            result[0].Count.ShouldBe(2);
            result[0].Find(c => c.ConnectorId == expectedConnectorId_1).Amps.ShouldBe(0.4m);
            result[0].Find(c => c.ConnectorId == expectedConnectorId_2).Amps.ShouldBe(0.5m);        
        }

        [Fact]
        public void Algo_should_return_multiple_double_options_with_min_current()
        {
            //Arrange
            decimal capacity = 10;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            var expectedConnectorId_1 = 9;
            var expectedConnectorId_2 = 8;
            var expectedConnectorId_3 = 5;
            chargeStation.AddConnector(0.4m, expectedConnectorId_1);
            chargeStation.AddConnector(0.5m, expectedConnectorId_2);
            chargeStation.AddConnector(0.9m, expectedConnectorId_3);
            chargeStation.AddConnector(2);
            chargeStation.AddConnector(3);
            var needToFree = 0.9m;
            //Act
            var result = new Algo().FindOptions(chargeGroup, needToFree);
            //Assert
            result.Count.ShouldBe(2);

            //0.4 + 0.5
            result.Find(l=>l.Exists(c=>c.ConnectorId == expectedConnectorId_1)).Count.ShouldBe(2);
            result.Find(l => l.Exists(c => c.ConnectorId == expectedConnectorId_1))
                .Find(c => c.ConnectorId == expectedConnectorId_1).Amps.ShouldBe(0.4m);
            result.Find(l => l.Exists(c => c.ConnectorId == expectedConnectorId_1))
                .Find(c => c.ConnectorId == expectedConnectorId_2).Amps.ShouldBe(0.5m);

            //0.9
            result.Find(l => l.Exists(c => c.ConnectorId == expectedConnectorId_3)).Count.ShouldBe(1);
            result.Find(l => l.Exists(c => c.ConnectorId == expectedConnectorId_3)).FirstOrDefault().Amps.ShouldBe(0.9m);
        }

    }
}
