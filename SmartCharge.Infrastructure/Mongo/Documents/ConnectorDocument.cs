namespace SmartCharge.Infrastructure.Mongo.Documents
{
    public sealed class ConnectorDocument
    {
        public int Id { get; set; }
        public decimal MaxCurrentAmps { get; set; }
        public string ParentChargeStationId { get; set; }
    }
}