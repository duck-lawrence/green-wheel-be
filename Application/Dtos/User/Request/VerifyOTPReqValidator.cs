using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class VerifyOTPReqValidator : AbstractValidator<VerifyOTPReq>
    {
        public VerifyOTPReqValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Message.UserMessage.EmailIsRequired)
                .EmailAddress().WithMessage(Message.UserMessage.InvalidEmail);

            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage(Message.UserMessage.OTPCanNotEmpty)
                .Matches(@"^\d{6}$").WithMessage(Message.UserMessage.OTPMustHave6Digits);
        }
    }
}
