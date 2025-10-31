using Application.Constants;
using Application.Dtos.BusinessVariable.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.BusinessVariable
{
    public class UpdateBusinessVariableReqValidator : AbstractValidator<UpdateBusinessVariableReq>
    {
        public UpdateBusinessVariableReqValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage(Message.BusinessVariable.ValueIsRequired)
                .GreaterThan(0).WithMessage(Message.BusinessVariable.ValueMustBeGreaterThanZero);
        }
    }
}
