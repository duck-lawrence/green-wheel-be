using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ticket.Request
{
    public class TicketFilterParams
    {
        public int? Status { get; set; }
        public int? Type { get; set; }
    }
}