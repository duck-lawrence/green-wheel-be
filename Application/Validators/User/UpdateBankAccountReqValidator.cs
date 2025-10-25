using Application.Constants;
using Application.Dtos.User.Request;
using CloudinaryDotNet.Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class UpdateBankAccountReqValidator : AbstractValidator<UpdateBankAccountReq>
    {
        public UpdateBankAccountReqValidator()
        {
            RuleFor(x => x.BankName)
                .NotEmpty().WithMessage(Message.UserMessage.BankNameIsRequired);

            RuleFor(x => x.BankAccountNumber)
                .NotEmpty().WithMessage(Message.UserMessage.BankAccountNumberIsRequired)
                .Matches(@"^\d*$").WithMessage(Message.UserMessage.InvalidBankAccountNumber);

            RuleFor(x => x.BankAccountName)
                .NotEmpty().WithMessage(Message.UserMessage.BankAccountNameIsRequired);
        }
    }
}