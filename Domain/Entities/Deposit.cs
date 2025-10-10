using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Deposit : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset? RefundedAt { get; set; }

    public int Status { get; set; }

    public Guid InvoiceId { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
