using Application.Dtos.User.Respone;
using Application.Dtos.Vehicle.Respone;
using Application.Dtos.VehicleChecklistItem.Respone;
using Domain.Entities;

namespace Application.Dtos.VehicleChecklist.Respone
{
    public class VehicleChecklistViewRes
    {
        public Guid Id { get; set; }

        public bool IsSignedByStaff { get; set; }

        public bool IsSignedByCustomer { get; set; }
        public UserProfileViewRes Staff { get; set; }

        public UserProfileViewRes? Customer { get; set; }

        public VehicleViewRes Vehicle { get; set; }

        public Guid? ContractId { get; set; }
      
        //public IEnumerable<ChecklistItemImageRes>? VehicleChecklistItems { get; set; }
        public IEnumerable<VehicleChecklistItemViewRes>? VehicleChecklistItems { get; set; }
    }
}
