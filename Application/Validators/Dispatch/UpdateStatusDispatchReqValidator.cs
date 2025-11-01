using Application.Dtos.Dispatch.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Validators.Dispatch
{
    public class UpdateStatusDispatchReqValidator : AbstractValidator<UpdateStatusDispatchReq>
    {
        public UpdateStatusDispatchReqValidator() 
        {
            RuleFor(x => x.Status)
                .InclusiveBetween(0, 3)
                .WithMessage(DispatchMessage.InvalidStatus);
        }
    }
}
