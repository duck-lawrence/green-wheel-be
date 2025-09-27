using Application.Dtos.User.Request;
using FluentValidation;
using Application.Constants;
namespace Application.Validators.User
{
    public class SendEmailReqValidator : AbstractValidator<SendEmailReq>
    {
        public SendEmailReqValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Message.User.EmailIsRequired)
                .EmailAddress().WithMessage(Message.User.InvalidEmail);

            
        }
    }
}
