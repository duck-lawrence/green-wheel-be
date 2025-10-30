using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Statistic.Responses
{
    public class TotalStatisticRes
    {
        public int TotalStatisticThisMonth {  get; set; }
        public int TotalStatisticLastMonth { get; set; }
        public decimal ChangeRate { get; set; }
    }
}
