using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum RentalContractStatus
    {
        RequestPeding = 0, 
        PaymentPending = 1,
        Active = 2,
        Completed = 3,
        Cancelled = 4,
        ConfirmChangeStationPending = 5,
    }
}
