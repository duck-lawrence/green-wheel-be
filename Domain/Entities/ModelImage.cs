using Domain.Commons;

namespace Domain.Entities;

public partial class ModelImage : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Url { get; set; } = null!;

    public string PublicId { get; set; } = null!;

    public Guid ModelId { get; set; }

    public virtual VehicleModel Model { get; set; } = null!;
}