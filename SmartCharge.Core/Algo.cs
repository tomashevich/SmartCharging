using SmartCharge.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCharge.Core
{
    public class Algo
    {
        private SortedList<decimal, List<List<ConnectorToUnplug>>> Results
            = new SortedList<decimal, List<List<ConnectorToUnplug>>>();

        private decimal OptimalAmpsToFree { get; set; }
        private ArrayList SortedConnectors = new ArrayList();

        public List<List<ConnectorToUnplug>> FindOptions(ChargeGroup chargeGroup, decimal needToFreeAmps)
        {
            GetInputData(chargeGroup);
            FindOptions(needToFreeAmps);

            var min = Results.Keys.Aggregate(decimal.MaxValue, (min, next) => next < min ? next : min);
            return Results[min];
        }

        private void FindOptions(decimal needToFreeAmps)
        {
            int inputLenght = SortedConnectors.Count;
            decimal minPossibleAmps = int.MaxValue;
           

            //look up for singles
            for (var i = 0; i <= inputLenght; i++)
            {
                var connector = (Connector)SortedConnectors[i];
                var currentValue = connector.MaxCurrentAmps;
                if (currentValue >= needToFreeAmps && currentValue <= minPossibleAmps)
                {
                    //found
                    minPossibleAmps = currentValue;

                    var UnplugOption = new List<ConnectorToUnplug>(){
                         new ConnectorToUnplug {
                                StationId = connector.ParentChargeStationId,
                                ConnectorId = connector.Id,
                                Amps = connector.MaxCurrentAmps }};

                    Results.AddToList(currentValue, UnplugOption);
                }
            }

            //look up for pairs

            for (var i = 0; i <= inputLenght-1; i++)
            {
                for (var j = i+1; j <= inputLenght; j++)
                {
                    var connectorI = (Connector)SortedConnectors[i];
                    var connectorJ = (Connector)SortedConnectors[j];

                    var currentValue = connectorI.MaxCurrentAmps + connectorJ.MaxCurrentAmps;
                    if (currentValue >= needToFreeAmps && currentValue <= minPossibleAmps)
                    {
                        //found
                        minPossibleAmps = currentValue;

                        var UnplugOption = new List<ConnectorToUnplug>(){
                            new ConnectorToUnplug {
                                StationId = connectorI.ParentChargeStationId,
                                ConnectorId = connectorI.Id,
                                Amps = connectorI.MaxCurrentAmps },
                            new ConnectorToUnplug {
                                StationId = connectorJ.ParentChargeStationId,
                                ConnectorId = connectorJ.Id,
                                Amps = connectorJ.MaxCurrentAmps },
                        };

                        Results.AddToList(currentValue, UnplugOption);
                    }
                }
            }

        }

        private void GetInputData(ChargeGroup chargeGroup)
        {
            SortedList<decimal, List<Connector>> Input = new SortedList<decimal, List<Connector>>();

            foreach (var chargeStation in chargeGroup.ChargeStations)
            {
                foreach (var connector in chargeStation.Connectors)
                {
                    Input.AddToList(connector);
                }
            }

            foreach (var kvp in Input)
            {
                foreach (var connector in kvp.Value)
                {
                    SortedConnectors.Add(connector);
                }
            }
        }



    }

    public static class Extensions
    {
        public static void AddToList(this SortedList<decimal, List<Connector>> input, Connector connectorToAdd)
        {
            var key = connectorToAdd.MaxCurrentAmps;
            if (input.ContainsKey(key))
            {
                input[key].Add(connectorToAdd);
            }
            else
            {
                input.Add(key, new List<Connector> { connectorToAdd });
            }
        }

        public static void AddToList(this SortedList<decimal, List<List<ConnectorToUnplug>>> input, decimal key, List<ConnectorToUnplug> unplugOption)
        {
            if (input.ContainsKey(key))
            {
                input[key].Add(unplugOption);
            }
            else
            {
                var optionsList = new List<List<ConnectorToUnplug>> { unplugOption };

                input.Add(key, optionsList);
            }
        }
    }

    public struct ConnectorToUnplug
    {

        public Guid StationId;
        public int ConnectorId;
        public decimal Amps;
    }
}
