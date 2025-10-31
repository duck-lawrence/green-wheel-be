using Application.Dtos.Brand.Request;
using Application.Dtos.CitizenIdentity.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Validators.Brand
{
    public class CreateBrandReqValidator : AbstractValidator<BrandReq>
    {
        public CreateBrandReqValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(BrandMessage.NameIsRequired);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(BrandMessage.DescriptionIsRequired);
            RuleFor(x => x.FoundedYear)
                .NotEmpty().WithMessage(BrandMessage.FoundedYearIsRequired);
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage(BrandMessage.FoundedYearIsRequired);
        }
    }
}
