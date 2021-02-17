using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCharge.Infrastructure.Mongo
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ChargeGroupCollectionName { get; set; }
        public string ChargeStationCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string ChargeGroupCollectionName { get; set; }
        string ChargeStationCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
