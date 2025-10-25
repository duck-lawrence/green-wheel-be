using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleModel.Respone
{
    public class VehicleModelImagesRes
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; } = null!;
        public IEnumerable<string> ImageUrls { get; set; } = Enumerable.Empty<string>();
    }
}