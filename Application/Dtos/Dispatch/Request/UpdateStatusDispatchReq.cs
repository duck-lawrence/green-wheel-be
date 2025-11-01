using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Dispatch.Request
{
    public class UpdateStatusDispatchReq
    {
        public int Status { get; set; }
        public string? Description { get; set; }
    }
}
