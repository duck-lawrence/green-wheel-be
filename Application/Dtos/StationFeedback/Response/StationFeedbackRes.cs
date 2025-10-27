using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.StationFeedback.Response
{
    public class StationFeedbackRes
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public Guid StationId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}