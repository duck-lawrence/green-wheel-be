using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum DepositStatus
    {
        Pending = 0,
        Paid = 1,
        Refunded = 2, 
        Forfeited = 3
    }
}
