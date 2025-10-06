using Application.Dtos.Invoice.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Respone
{
    public class RentalContractForStaffViewRes
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string Description { get; set; } = null!;

        public string? Notes { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? ActualStartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public DateTimeOffset? ActualEndDate { get; set; }

        public IEnumerable<InvoiceViewRes> Invoices { get; set; }
        public bool IsSignedByStaff { get; set; }

        public bool IsSignedByCustomer { get; set; }

        public int Status { get; set; }

        public Guid? VehicleId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? HandoverStaffId { get; set; }

        public Guid? ReturnStaffId { get; set; }
    }
}
