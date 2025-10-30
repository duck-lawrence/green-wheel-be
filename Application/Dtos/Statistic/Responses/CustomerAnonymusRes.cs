using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Statistic.Responses
{
    public class CustomerAnonymusRes
    {
        public int CustomerAnonymusInThisMonth { get; set; }
        public int CustomerAnonymusInLastMonth { get; set; }
        public decimal ChangeRate { get; set; }
    }
}
