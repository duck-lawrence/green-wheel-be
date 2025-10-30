using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class StatisticHelper
    {
        public static int GetLastMonth()
        {
            var now = DateTimeOffset.UtcNow;
            return now.Month == 1 ? 12 : now.Month - 1;
        }

        public static int GetLastMonthYear()
        {
            var now = DateTimeOffset.UtcNow;
            return now.Month == 1 ? now.Year - 1 : now.Year;
        }
    }
}
