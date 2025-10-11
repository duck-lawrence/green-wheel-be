using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklistItem.Respone
{
    public class ChecklistItemImageRes
    {
        public Guid ItemId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}