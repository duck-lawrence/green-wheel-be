using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class VehicleModel : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal CostPerDay { get; set; }

    public decimal DepositFee { get; set; }

    public int SeatingCapacity { get; set; }

    public int NumberOfAirbags { get; set; }

    public decimal MotorPower { get; set; }

    public decimal BatteryCapacity { get; set; }

    public decimal EcoRangeKm { get; set; }

    public decimal SportRangeKm { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImagePublicId { get; set; }

    public Guid BrandId { get; set; }

    public Guid SegmentId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<ModelComponent> ModelComponents { get; set; } = new List<ModelComponent>();

    public virtual ICollection<ModelImage> ModelImages { get; set; } = new List<ModelImage>();

    public virtual VehicleSegment Segment { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}