using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Brand : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int FoundedYear { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
}
