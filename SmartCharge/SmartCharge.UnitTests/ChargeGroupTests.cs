using Shouldly;
using SmartCharge.Core.Entities;
using SmartCharge.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartCharge.UnitTests
{
    public class ChargeGroupTests
    {
        [Fact]
        public void Capacity_should_be_grater_than_0()
        {
            //Arrange
            decimal capacity = 0;

            //Act
            //Assert
            var ex = Assert.Throws<InvalidChargeGroupCapacity>(
                () => new ChargeGroup(Guid.NewGuid(),"", capacity, null));
        }

        [Fact]
        public void ChargeGroupId_should_be_newGuid_by_default()
        {
            //Arrange
            decimal capacity = 10;

            //Act
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            //Assert
            chargeGroup.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void chargeStations_should_be_not_null_by_default()
        {
            //Arrange
            decimal capacity = 10;

            //Act
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            //Assert
            chargeGroup.ChargeStations.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateCapasity_should_return_suggestions_if_capacity_reduced_too_much()
        {
            //Arrange
            decimal capacity = 10;
            decimal newCapacity = 3;
            decimal connectorMaxCurrent = 5;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeStation.AddConnector(connectorMaxCurrent);
            chargeGroup.AddChargeStation(chargeStation);

            //Act
            var result = chargeGroup.UpdateCapacity(newCapacity);
            //Assert
            result.IsError.ShouldBeTrue();
            result.Suggestions.ShouldNotBeNull();
            result.Suggestions.ShouldNotBeEmpty();
            result.Suggestions.FirstOrDefault().FirstOrDefault().Amps.ShouldBe(connectorMaxCurrent);

        }

        [Fact]
        public void AddChargeStation_should_return_suggestions_if_capacity_reserve_exceeded()
        {
            //Arrange
            decimal capacity = 10;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeStation.AddConnector(6);
            var chargeStation2 = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeStation2.AddConnector(5);
            chargeGroup.AddChargeStation(chargeStation);

            //Act
            var result = chargeGroup.AddChargeStation(chargeStation2);
            //Assert
            result.IsError.ShouldBeTrue();
            result.Suggestions.ShouldNotBeNull();
            result.Suggestions.ShouldNotBeEmpty();
            result.Suggestions.FirstOrDefault().FirstOrDefault().Amps.ShouldBe(6);

        }

        [Fact]
        public void CapacityReserve_should_sum_all_connectors_in_all_stations_in_group()
        {
            //Arrange
            decimal capacity = 10;
            var chargeGroup = new ChargeGroup(Guid.Empty, "", capacity, null);
            var chargeStation = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeStation.AddConnector(1);
            chargeStation.AddConnector(2);
            var chargeStation2 = new ChargeStation(Guid.NewGuid(), "", chargeGroup);
            chargeStation2.AddConnector(3);
            chargeStation2.AddConnector(0.5m);
            chargeGroup.AddChargeStation(chargeStation);
            chargeGroup.AddChargeStation(chargeStation2);
            //Act
            var result = chargeGroup.CapacityReserve;
            //Assert
            result.ShouldBe(10 - (1 + 2 + 3 + 0.5m));

        }
    }
}
