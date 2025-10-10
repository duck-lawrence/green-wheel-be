using Domain.Commons;

namespace Domain.Entities
{
    public class Ticket : SorfDeletedEntity, IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Reply { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Guid CustomerId { get; set; }
        public Guid? AssigneeId { get; set; }

        public virtual User Customer { get; set; } = null!;
        public virtual Staff? Assignee { get; set; }
    }
}