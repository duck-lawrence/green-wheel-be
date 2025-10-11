using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum InvoiceType
    {
        Handover = 0,
        Return = 1,
        Refund = 2,
        Reservation = 3,
        Other = 4
    }
}
