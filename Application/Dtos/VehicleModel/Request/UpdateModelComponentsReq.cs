using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleModel.Request
{
    public class UpdateModelComponentsReq
    {
        public IEnumerable<Guid> ComponentIds { get; set; } = [];
    }
}
