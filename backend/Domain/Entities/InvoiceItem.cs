using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class InvoiceItem : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string? Notes { get; set; }

    public int Type { get; set; }

    public Guid InvoiceId { get; set; }

    public Guid? ChecklistItemId { get; set; }

    public virtual VehicleChecklistItem? ChecklistItem { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
