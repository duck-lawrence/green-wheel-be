using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Dispatch.Request
{
    public class CreateDispatchReqValidator : AbstractValidator<CreateDispatchReq>
    {
        public CreateDispatchReqValidator()
        {
            RuleFor(x => x.FromStationId)
                .NotEmpty().WithMessage("FromStationId is required.");
        }
    }
}
