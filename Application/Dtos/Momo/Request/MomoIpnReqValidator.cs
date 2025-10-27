using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Request
{
    public class MomoIpnReqValidator : AbstractValidator<MomoIpnReq>
    {
        public MomoIpnReqValidator()
        {
            RuleFor(x => x.PartnerCode).NotEmpty();
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.RequestId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.OrderInfo).NotEmpty();
            RuleFor(x => x.OrderType).NotEmpty();
            RuleFor(x => x.TransId).GreaterThan(0);
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.PayType).NotEmpty();
            RuleFor(x => x.ResponseTime).GreaterThan(0);
            RuleFor(x => x.Signature).NotEmpty();
        }
    }
}
