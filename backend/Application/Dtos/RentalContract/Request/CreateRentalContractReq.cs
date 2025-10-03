using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public  class CreateRentalContractReq
    {
        public Guid ModelId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Guid StationId { get; set; }
    }
}
