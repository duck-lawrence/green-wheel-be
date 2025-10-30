using Application.Constants;
using Application.Dtos.RentalContract.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.RentalContract
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
