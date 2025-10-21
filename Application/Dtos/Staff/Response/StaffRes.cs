using Application.Dtos.Station.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Staff.Response
{
    public class StaffRes
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string? Phone { get; init; }
        public StationSimpleRes Station { get; init; } = default!;
    }
}
