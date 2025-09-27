using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StationFeedback : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? Content { get; set; }

    public int Rating { get; set; }

    public Guid CustomerId { get; set; }

    public Guid StationId { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
