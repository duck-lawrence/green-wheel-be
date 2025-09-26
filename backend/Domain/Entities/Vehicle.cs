using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Vehicle : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string LicensePlate { get; set; } = null!;

    public int Status { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public Guid ModelId { get; set; }

    public Guid StationId { get; set; }

    public virtual ICollection<DispatchRequestVehicle> DispatchRequestVehicles { get; set; } = new List<DispatchRequestVehicle>();

    public virtual VehicleModel Model { get; set; } = null!;

    public virtual ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();

    public virtual Station Station { get; set; } = null!;

    public virtual ICollection<VehicleChecklist> VehicleChecklists { get; set; } = new List<VehicleChecklist>();

    public virtual ICollection<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();
}
