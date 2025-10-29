using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Statistic.Responses
{
    public class TotalRevenueRes
    {
        public Decimal TotalRevenueThisMonth {  get; set; }
        public Decimal TotalRevenueLastMonth {  get; set; }
        public decimal ChangeRate { get; set; }

    }
}
