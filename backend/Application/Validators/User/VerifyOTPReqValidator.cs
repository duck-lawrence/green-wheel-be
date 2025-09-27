using Application.Constants;
using Application.Dtos.User.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class VerifyOTPReqValidator : AbstractValidator<VerifyOTPReq>
    {
        public VerifyOTPReqValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Message.User.EmailIsRequired)
                .EmailAddress().WithMessage(Message.User.InvalidEmail);

            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage(Message.User.OTPCanNotEmpty)
                .Length(6).WithMessage(Message.User.OTPMustHave6Digits);
        }
    }
}
