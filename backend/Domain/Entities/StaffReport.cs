using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StaffReport : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Reply { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    

    public Guid? SupportRequestId { get; set; }

    public Guid StaffId { get; set; }

    public Guid? AdminId { get; set; }

    public virtual Staff? Admin { get; set; }

    public virtual Staff Staff { get; set; } = null!;

    public virtual SupportRequest? SupportRequest { get; set; }
}
