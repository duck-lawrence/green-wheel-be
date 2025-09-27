using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class SupportRequest : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Reply { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }
    public Guid CustomerId { get; set; }

    public Guid? StaffId { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<StaffReport> StaffReports { get; set; } = new List<StaffReport>();
}
