using Application.Dtos.VehicleChecklistItem.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklist.Request
{
    public class UpdateVehicleChecklistReq
    {
        public bool IsSignByStaff { get; set; }
        public bool IsSignByCustomer { get; set; }
        public Guid VehicleChecklistId { get; set; }
        public string? Description { get; set; }
        public IEnumerable<UpdateChecklistItemReq> ChecklistItems { get; set; }
    }
}
