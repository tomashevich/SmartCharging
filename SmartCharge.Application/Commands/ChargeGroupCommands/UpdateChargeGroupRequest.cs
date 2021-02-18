namespace SmartCharge.Application.Commands.ChargeGroupCommands
{
    public class UpdateChargeGroupRequest
    {
        public string Name { get; set; }
        public decimal CapacityAmps { get; set; }
    }
}
