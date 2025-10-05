using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class LisenceHelper
    {
        public static int MapLicenseClass(string cls)
        {
            return cls.ToUpper() switch
            {
                "B1" => 0,
                "B" => 1,
                "C1" => 2,
                "C" => 3,
                "D1" => 4,
                "D2" => 5,
                "D" => 6,
                "BE" => 7,
                "C1E" => 8,
                "CE" => 9,
                "D1E" => 10,
                "D2E" => 11,
                "DE" => 12,
                _ => -1 // unknown
            };
        }
    }
}
