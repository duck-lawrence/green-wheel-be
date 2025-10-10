using Application.Dtos.Deposit.Respone;
using Application.Dtos.Invoice.Response;
using Application.Dtos.Station.Respone;
using Application.Dtos.Vehicle.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Respone
{
    public class RentalContractViewRes
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? ActualStartDate { get; set; } = null;
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset? ActualEndDate { get; set; } = null;
        public int Status { get; set; }
        public VehicleViewRes Vehicle { get; set; }
        public Guid CustomerId { get; set; }
        public StationViewRes Station { get; set; }
        public Guid? HandoverStaffId { get; set; } = null;
        public Guid? ReturnStaffId { get; set; } = null;
        public IEnumerable<InvoiceViewRes> Invoices { get; set; }
        


    }
}
