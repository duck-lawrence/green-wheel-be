using Application.Constants;
using Application.Dtos.VehicleSegment.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.VehicleSegment
{
    public class CreateSegmentReqValidation : AbstractValidator<CreateSegmentReq>
    {
        public CreateSegmentReqValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Message.VehicleSegmentMessage.NameIsRequired);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(Message.VehicleSegmentMessage.DescriptionIsRequired);
        }
    }
}
