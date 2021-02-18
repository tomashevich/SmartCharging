using Shouldly;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SmartCharge.UnitTests
{
    public class ChargeStationTests
    {
        [Fact]
        public void ChargeStationId_should_be_newGuid_by_default()
        {
            //Arrange
            //Act
            var chargestation = new ChargeStation(Guid.Empty, "", null);
            //Assert
            chargestation.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void MaxCurrentAmps_should_sum_all_connectors()
        {
            //Arrange
            decimal groupCapacity = 10m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            chargeStation.AddConnector(1.0m);
            chargeStation.AddConnector(0.2m);
            chargeStation.AddConnector(0.03m);
            //Act
            var expected = chargeStation.MaxCurrentAmps;
            //Assert
            expected.ShouldBe(1.23m);
        }

        [Fact]
        public void Connector_id_should_be_initialised_implicitly()
        {
            //Arrange
            decimal groupCapacity = 10m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            chargeStation.AddConnector(0.3m);
            //Act
            var connector = chargeStation.Connectors.FirstOrDefault();
            //Assert
            connector.ShouldNotBeNull();
            connector.Id.ShouldBe(1);
            connector.MaxCurrentAmps.ShouldBe(0.3m);
            connector.ParentChargeStationId.ShouldBe(chargeStation.Id);
        }

        [Fact]
        public void Connector_id_can_be_initialised_explicitly()
        {
            //Arrange
            decimal groupCapacity = 10m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            int connectorId = 2;
            chargeStation.AddConnector(0.3m, connectorId);
            //Act
            var connector = chargeStation.Connectors.FirstOrDefault();
            //Assert
            connector.ShouldNotBeNull();
            connector.Id.ShouldBe(2);
            connector.MaxCurrentAmps.ShouldBe(0.3m);
            connector.ParentChargeStationId.ShouldBe(chargeStation.Id);
        }

        [Fact]
        public void Connector_id_can_not_be_grater_than_MAX_CONNECTOR_ID_VALUE()
        {
            //Arrange
            decimal groupCapacity = 10m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            int connectorId = Const.MAX_CONNECTOR_ID_VALUE + 1;
            //Act
            //Assert
            var ex = Assert.Throws<InvalidConnectorId>(
               () => chargeStation.AddConnector(0.3m, connectorId));         
        }

        [Fact]
        public void Connectors_number_can_not_be_grater_than_MAX_CONNECTORS()
        {
            //Arrange
            decimal groupCapacity = 10m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);

            for (int i = 0; i < Const.MAX_CONNECTORS; i++)
            {
                chargeStation.AddConnector(1.0m);
            }
            //Act
            //Assert
            var ex = Assert.Throws<InvalidChargeStationConnectorsAmount>(
               () => chargeStation.AddConnector(0.3m));
        }

        [Fact]
        public void AddConnector_should_return_suggestions_if_capacity_reserve_exceeded()
        {
            //Arrange
            decimal groupCapacity = 2.0m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            chargeStation.AddConnector(1.0m);
            chargeStation.AddConnector(0.2m);
           
            //Act
            var result = chargeStation.AddConnector(1.0m);
            //Assert
            result.IsError.ShouldBeTrue();
            result.Suggestions.ShouldNotBeNull();
            result.Suggestions.ShouldNotBeEmpty();
            result.Suggestions.FirstOrDefault().FirstOrDefault().Amps.ShouldBe(0.2m);
        }

        [Fact]
        public void ChangeConnectorMaxCurrent_should_return_suggestions_if_capacity_reserve_exceeded()
        {
            //Arrange
            decimal groupCapacity = 2.0m;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", groupCapacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeGroup.AddChargeStation(chargeStation);
            var connectorId = 1;
            chargeStation.AddConnector(1.0m, connectorId);
            chargeStation.AddConnector(0.2m);

            //Act
            var result = chargeStation.ChangeConnectorMaxCurrent(connectorId, 2.0m);
            //Assert
            result.IsError.ShouldBeTrue();
            result.Suggestions.ShouldNotBeNull();
            result.Suggestions.ShouldNotBeEmpty();
            result.Suggestions.FirstOrDefault().FirstOrDefault().Amps.ShouldBe(0.2m);
        }
    }
}
