using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Ticket.Request
{
    public class CreateTicketReqValidator : AbstractValidator<CreateTicketReq>
    {
        public CreateTicketReqValidator() 
        {
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage(Message.TicketMessage.TitleRequired)
                .MaximumLength(255).WithMessage(Message.TicketMessage.TitleTooLong);
            RuleFor(x => x.Description).NotEmpty()
                .WithMessage(Message.TicketMessage.DescriptionRequired);
            RuleFor(x => x.Type).InclusiveBetween(0, 4).WithMessage(Message.TicketMessage.InvalidType);
        }
    }
}
