using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Role : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
