using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Station.Respone;
using Domain.Entities;

namespace Application.Dtos.User.Respone
{
    public class UserProfileViewRes
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Sex { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Phone { get; set; }
        public string? LicenseUrl { get; set; }
        public string? CitizenUrl { get; set; }
        public Role? Role { get; set; }
        public StationViewRes? Station { get; set; }
    }
}

