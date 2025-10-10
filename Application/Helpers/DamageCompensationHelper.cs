using Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class DamageCompensationHelper
    {
        public static decimal CalculateCompensation(decimal baseAmount, int damageStatus)
        {
            decimal multiplier = damageStatus switch
            {
                (int)DamageStatus.Good => 0.0m,        // Không hư hỏng -> không đền bù
                (int)DamageStatus.Minor => 0.05m,      // Nhẹ -> 5%
                (int)DamageStatus.Moderate => 0.15m,   // Vừa -> 15%
                (int)DamageStatus.Severe => 0.4m,      // Nặng -> 40%
                (int)DamageStatus.Totaled => 1.0m,     // Hư hại toàn phần -> 100%
                _ => 0.0m
            };

            return baseAmount * multiplier;
        }
    }
}
