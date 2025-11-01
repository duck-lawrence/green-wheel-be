using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum DispatchRequestStatus
    {
        Pending = 0,
        Approved = 1,
        ConfirmApproved = 2,
        Rejected = 3,
        Received = 4,
        Cancelled = 5
    }
}