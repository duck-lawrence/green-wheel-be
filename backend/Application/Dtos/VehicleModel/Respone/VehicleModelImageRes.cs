using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleModel.Respone
{
    public class VehicleModelImageRes
    {
        public Guid ModelId { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}