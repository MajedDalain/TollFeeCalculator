namespace TollFeeCalculator.Services.TollFreeConditions
{
    public class TollFreeVehicle : ITollFreeCondition
    {
        private static readonly HashSet<string> TollFreeVehicleTypes = new HashSet<string>
        {
            "Motorbike",
            "Tractor",
            "Emergency",
            "Diplomat",
            "Foreign",
            "Military"
        };

        public bool IsTollFree(Vehicle vehicle, DateTime date)
        {
            return IsVehicleTypeTollFree(vehicle.GetVehicleType());

        }

        private bool IsVehicleTypeTollFree(string vehicleType)
        {
            return TollFreeVehicleTypes.Contains(vehicleType);
        }
    }

}
