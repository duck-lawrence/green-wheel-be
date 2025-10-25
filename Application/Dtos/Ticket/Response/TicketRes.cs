using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.User.Respone;

namespace Application.Dtos.Ticket.Response
{
    public class TicketRes
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Reply { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public UserProfileViewRes? Requester { get; set; }
        public UserProfileViewRes? Assignee { get; set; }
    }
}