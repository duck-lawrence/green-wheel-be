using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Ticket : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Reply { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public Guid? StationId { get; set; }
    public virtual Station? Station { get; set; }

    public Guid? RequesterId { get; set; }
    public virtual User? Requester { get; set; } = null!;

    public Guid? AssigneeId { get; set; }
    public virtual Staff? Assignee { get; set; }
}