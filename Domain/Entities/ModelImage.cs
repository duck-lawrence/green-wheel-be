using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ModelImage : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;

    public string PublicId { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

   

    public Guid ModelId { get; set; }

    public virtual VehicleModel Model { get; set; } = null!;
}
