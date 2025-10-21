using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Vehicle.Respone
{
    public class VehicleModelRes
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
