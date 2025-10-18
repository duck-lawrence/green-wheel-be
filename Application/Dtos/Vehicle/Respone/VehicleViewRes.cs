using Application.Dtos.VehicleModel.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Vehicle.Respone
{
    public class VehicleViewRes
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public Guid StationId { get; set; }
        public VehicleModelViewRes Model { get; set; } = null!;
        public int Status {get; set;}
    }
}
