using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleModel.Request
{
    public class VehicleFilterReqValidator : AbstractValidator<VehicleFilterReq>
    {
        public VehicleFilterReqValidator()
        {
            RuleFor(x => x.StationId)
                .NotEqual(Guid.Empty)
                .WithMessage(Message.VehicleMessage.StationIdRequired);


            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage(Message.VehicleModelMessage.RentTimeIsNotAvailable);
        }
    }
}
