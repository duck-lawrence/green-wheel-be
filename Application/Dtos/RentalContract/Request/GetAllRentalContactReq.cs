using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public class GetAllRentalContactReq
    {
        public string? DriverLicenseNumber { get; set; }
        public string? CitizenIdentityNumber { get; set; }
        public string? Phone { get; set; }
        public int? Status { get; set; }
        public Guid? StationId { get; set; }
    }
}
