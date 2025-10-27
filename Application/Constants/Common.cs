using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public static class Common
    {
        public static class Fee
        {
            public const decimal LateReturn = 10000;
            public const decimal Cleaning = 10000;
        }

        public static class Tax
        {
            public const decimal BaseVAT = 0.10m;
            public const decimal NoneVAT = 0m;

        }

        public static class Policy
        {
            public const int MaxLateHours = 1;
        }

        public static class Day
        {
            public const int RentalContractBufferDay = 10;
        }
    }
}
