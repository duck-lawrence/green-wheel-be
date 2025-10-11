using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class VehicleChecklist : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public int Type { get; set; }

    public bool IsSignedByStaff { get; set; }

    public bool IsSignedByCustomer { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public Guid StaffId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid VehicleId { get; set; }

    public Guid? ContractId { get; set; }

    public virtual RentalContract? Contract { get; set; }

    public virtual User? Customer { get; set; }

    public virtual Staff Staff { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;

    public virtual ICollection<VehicleChecklistItem> VehicleChecklistItems { get; set; } = new List<VehicleChecklistItem>();
}
