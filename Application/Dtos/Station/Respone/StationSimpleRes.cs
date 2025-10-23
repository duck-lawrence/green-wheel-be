using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Station.Respone
{
    public class StationSimpleRes
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string Address { get; init; } = default!;

        public DateTimeOffset createAt { get; set;} 
    }
}
