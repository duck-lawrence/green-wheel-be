using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Respone
{
    public class UserProfileViewRes
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LicenseUrl { get; set; }
        public string? CitizenUrl { get; set; }
    }
}
