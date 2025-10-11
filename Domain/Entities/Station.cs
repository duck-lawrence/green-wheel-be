using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Station : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

   

    public virtual ICollection<DispatchRequest> DispatchRequestFromStations { get; set; } = new List<DispatchRequest>();

    public virtual ICollection<DispatchRequest> DispatchRequestToStations { get; set; } = new List<DispatchRequest>();

    public virtual ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<StationFeedback> StationFeedbacks { get; set; } = new List<StationFeedback>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
