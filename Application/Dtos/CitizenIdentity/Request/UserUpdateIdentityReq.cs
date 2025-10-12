using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.CitizenIdentity.Request
{
    public class UserUpdateIdentityReq
    {
        public string Number { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public int Sex { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
    }
}