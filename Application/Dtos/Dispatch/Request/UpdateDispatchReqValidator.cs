using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Dtos.Dispatch.Request
{
    public class UpdateDispatchReqValidator : AbstractValidator<UpdateDispatchReq>
    {
        public UpdateDispatchReqValidator()
        {
            RuleFor(x => x.Status)
                .InclusiveBetween(0, 3) 
                .WithMessage(DispatchMessage.InvalidStatus);
        }
    }
}
