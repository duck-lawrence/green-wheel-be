using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Brand.Request
{
    public class BrandFilterReq
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; } = null!;

        public string? Description { get; set; } = null!;

        public string? Country { get; set; } = null!;

        public int? FoundedYear { get; set; }
    }
}
