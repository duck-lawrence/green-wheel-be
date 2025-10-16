﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Payment.Request
{
    public class PaymentReq
    {
        public int PaymentMethod { get; set; }
        public string FallbackUrl { get; set; } = null!;
        public decimal? Amount { get; set; } = null;
    }
}
