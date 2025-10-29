using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons
{
    public class VehicleStatusCountItem
    {
        public int Status { get; set; }
        public int NumberOfVehicle { get; set; }
        public VehicleStatusCountItem(int status, int NumberOfVehicle) 
        { 
            this.Status = status;
            this.NumberOfVehicle = NumberOfVehicle;
        }
    }
}
