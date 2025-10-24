using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public class ConfirmReq
    {
        public Guid Id { get; set; }
        public bool HasVehicle { get; set; }
        public int? VehicleStatus { get; set; } = null;
    }
}
