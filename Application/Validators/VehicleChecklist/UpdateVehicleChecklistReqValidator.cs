using FluentValidation;
using Application.Constants;
using Application.Dtos.VehicleChecklist.Request;
using Application.Dtos.VehicleChecklistItem.Request;

namespace Application.Validators.VehicleChecklist
{
    public class UpdateVehicleChecklistReqValidator : AbstractValidator<UpdateVehicleChecklistReq>
    {
        public UpdateVehicleChecklistReqValidator()
        {
            RuleFor(x => x)
                .Must(x => x.IsSignedByStaff || x.IsSignedByCustomer)
                .WithMessage(Message.VehicleChecklistMessage.AtLeastOnePartyMustSign);

            RuleFor(x => x.ChecklistItems)
                .NotEmpty().WithMessage(Message.VehicleChecklistMessage.NotFound);

            RuleForEach(x => x.ChecklistItems)
                .SetValidator(new UpdateChecklistItemReqValidator());
        }
    }
}
