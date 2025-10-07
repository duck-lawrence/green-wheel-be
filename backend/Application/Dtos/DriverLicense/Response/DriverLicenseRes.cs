using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.DriverLicense.Response
{
    public class DriverLicenseRes
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public string Sex { get; set; }
        public string DateOfBirth { get; set; } = null!;
        public string ExpiresAt { get; set; } = null!;
        public string ImagePublicId { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}