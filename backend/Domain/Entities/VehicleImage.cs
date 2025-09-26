using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class VehicleImage : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Url { get; set; } = null!;

    public string PublicId { get; set; } = null!;

    

    public Guid VehicleId { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
