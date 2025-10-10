using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class DispatchRequestStaff : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public Guid DispatchRequestId { get; set; }

    public Guid StaffId { get; set; }

    public virtual DispatchRequest DispatchRequest { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
