using Application.Dtos.VehicleChecklistItem.Respone;

namespace Application.Dtos.VehicleChecklist.Respone
{
    public class VehicleChecklistViewRes
    {
        public Guid Id { get; set; }

        public bool IsSignedByStaff { get; set; }

        public bool IsSignedByCustomer { get; set; }
        public Guid StaffId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid VehicleId { get; set; }

        public Guid? ContractId { get; set; }
        public IEnumerable<VehicleChecklistItemViewRes>? VehicleChecklistItems { get; set; }
    }
}
