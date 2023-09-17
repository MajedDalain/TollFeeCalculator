using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Models
{
    internal class ElectricVehicle: Vehicle
    {
        public string GetVehicleType()
        {
            return "Electric";
        }
    }
}
