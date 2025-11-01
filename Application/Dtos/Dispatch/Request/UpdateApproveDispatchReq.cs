using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Dispatch.Request
{
    public sealed class UpdateApproveDispatchReq
    {
        public int Status { get; set; }
        public Guid[]? StaffId { get; set; }
        public Guid[]? VehicleId {get; set; }
        public string? Description { get; set; }
    }
}
