using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.DriverLicense.Request
{
    public class UpdateDriverLicenseReq
    {
        public string? Number { get; set; }
        public int? Class { get; set; }
        public string? FullName { get; set; }
        public string? Nationality { get; set; }
        public int? Sex { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}