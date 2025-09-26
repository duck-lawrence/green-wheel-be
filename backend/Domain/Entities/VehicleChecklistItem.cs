using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class VehicleChecklistItem
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? Notes { get; set; }

    public int Status { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public Guid ComponentId { get; set; }

    public Guid ChecklistId { get; set; }

    public virtual VehicleChecklist Checklist { get; set; } = null!;

    public virtual VehicleComponent Component { get; set; } = null!;

    public virtual InvoiceItem? InvoiceItem { get; set; }
}
