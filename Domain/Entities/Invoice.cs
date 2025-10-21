using Domain.Commons;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Invoice : SorfDeletedEntity, IEntity
{
    public Guid Id { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Tax { get; set; }

    public decimal? PaidAmount { get; set; }

    public int PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }
    public string ImageUrl { get; set; } = null!;

    public string ImagePublicId { get; set; } = null!;

    public DateTimeOffset? PaidAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public Guid ContractId { get; set; }

    public virtual RentalContract Contract { get; set; } = null!;

    public virtual Deposit? Deposit { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
