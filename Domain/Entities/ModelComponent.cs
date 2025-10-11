using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ModelComponent : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

   

    public Guid ModelId { get; set; }

    public Guid ComponentId { get; set; }

    public virtual VehicleComponent Component { get; set; } = null!;

    public virtual VehicleModel Model { get; set; } = null!;
}
