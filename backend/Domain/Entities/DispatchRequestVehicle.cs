using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class DispatchRequestVehicle : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public Guid DispatchRequestId { get; set; }

    public Guid VehicleId { get; set; }

    public virtual DispatchRequest DispatchRequest { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
