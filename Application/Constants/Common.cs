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
        public static class Tax
        {
            public const decimal NoneVAT = 0m;
        }
        public enum SystemCache
        {
            AllRoles = 0,
            BusinessVariables = 1,
        }
    }
}
