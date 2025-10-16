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
        Rejected = 2,
        Received = 3,
        Cancel = 4,
    }
}