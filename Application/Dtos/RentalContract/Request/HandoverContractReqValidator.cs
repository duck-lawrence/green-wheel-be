using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public class HandoverContractReqValidator : AbstractValidator<HandoverContractReq>
    {
        public HandoverContractReqValidator()
        {
            RuleFor(x => x)
                .Must(x => x.IsSignedByCustomer || x.IsSignedByStaff)
                .WithMessage(Message.RentalContractMessage.AtLeastOnePartyMustSign);
        }
    }
}
