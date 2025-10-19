using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum InvoiceItemType
    {
        BaseRental = 0,
        Damage = 1,
        LateReturn = 2,
        Cleaning = 3,
        Penalty = 4,
        Refund = 5,
        Other = 6
    }
}
