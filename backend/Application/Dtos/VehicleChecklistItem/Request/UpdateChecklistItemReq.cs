using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklistItem.Request
{
    public class UpdateChecklistItemReq
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public string? Notes { get; set; }
    }
}
