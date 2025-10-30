using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Statistic.Responses
{
    public class CustomerRes
    {
        public int CustomerInThisMonth { get; set; }
        public int CustomerInLastMonth { get; set; }
        public decimal ChangeRate { get; set; }
    }
}
