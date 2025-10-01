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
        ConfirmChangeStationPending = 1,
        PaymentPending = 2,
        Active = 3,
        Completed = 4,
        Cancelled = 5
    }
}
