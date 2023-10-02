namespace TollFeeCalculator.Services.TollFreeConditions
{
    // this can be made as two different interfaces, one for ITollFreeDateCondition and one for ITollFreeVehicleCondition , better adher to ISP 
    // a class does not need to implement an interface that does not use. 
    public interface ITollFreeCondition
    {
        bool IsTollFree(Vehicle vehicle, DateTime date);
    }

}
 