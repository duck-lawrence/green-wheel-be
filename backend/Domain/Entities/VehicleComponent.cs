using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class VehicleComponent : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    

    public virtual ICollection<ModelComponent> ModelComponents { get; set; } = new List<ModelComponent>();

    public virtual ICollection<VehicleChecklistItem> VehicleChecklistItems { get; set; } = new List<VehicleChecklistItem>();
}
