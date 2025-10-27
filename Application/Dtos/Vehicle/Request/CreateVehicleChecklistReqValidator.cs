using Application.Dtos.VehicleChecklist.Request;
using FluentValidation;
using Application.Constants;

namespace Application.Dtos.Vehicle.Request
{
    public class CreateVehicleChecklistReqValidator : AbstractValidator<CreateVehicleChecklistReq>
    {
        public CreateVehicleChecklistReqValidator()
        {
            RuleFor(x => x.Type)
                .InclusiveBetween(0, 2)
                .WithMessage(Message.VehicleChecklistMessage.InvalidStatus);
        }
    }
}