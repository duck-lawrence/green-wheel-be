using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklist.Request
{
    public class CreateVehicleChecklistReq
    {
        public Guid? CustomerId { get; set; }
        public Guid? ContractId { get; set; }
        public Guid VehicleId { get; set; }
    }
}
