namespace SmartCharge.Application.Commands.ChargeStationCommands
{
    public class ConnectorRequest
    {
        public int ConnectorId { get; set; }
        public decimal MaxCurrentAmps { get; set; }
    }
}
