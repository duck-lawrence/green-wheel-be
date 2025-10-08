using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum VehicleStatus
    {
        Available = 0,
        Unavaible = 1,
        Rented = 2,
        Maintenance = 3,
        MissingNoReason = 4,
        LateReturn = 5
    }
}
