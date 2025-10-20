using Application.Dtos.User.Respone;
using Application.Dtos.Vehicle.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Dispatch.Response
{
    public class DispatchRequestStaffRes
    {
        public Guid StaffId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public UserProfileViewRes Staff { get; init; } = default!;
    }
}
