using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class CitizenIdentity : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public string Number { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public int Sex { get; set; }

    public DateTimeOffset DateOfBirth { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string ImagePublicId { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }


    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
