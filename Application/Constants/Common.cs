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
            public const decimal Cleaning = 10000;
        }

        public static class Tax
        {
            public const decimal NoneVAT = 0m;
        }
    }
}
