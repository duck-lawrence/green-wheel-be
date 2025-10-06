using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class RentalContract : SorfDeletedEntity, IEntity
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

    public bool IsSignedByStaff { get; set; }

    public bool IsSignedByCustomer { get; set; }

    public int Status { get; set; }

    public Guid? VehicleId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? HandoverStaffId { get; set; }

    public Guid? ReturnStaffId { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual Staff? HandoverStaff { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Staff? ReturnStaff { get; set; }

    public virtual Vehicle? Vehicle { get; set; }

    public virtual ICollection<VehicleChecklist> VehicleChecklists { get; set; } = new List<VehicleChecklist>();
}
