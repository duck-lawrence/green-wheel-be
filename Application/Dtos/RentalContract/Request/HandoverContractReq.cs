using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public class HandoverContractReq
    {
        public bool IsSignedByStaff { get; set; }
        public bool IsSignedByCustomer { get; set; }
    }
}
