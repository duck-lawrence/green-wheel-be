using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public class HandoverContractReq
    {
        public bool IsSignByStaff { get; set; }
        public bool IsSignByCustomer { get; set; }
        public decimal? Amount { get; set; } = null;
    }
}
