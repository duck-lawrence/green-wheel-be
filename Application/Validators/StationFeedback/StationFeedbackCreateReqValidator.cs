using Application.Constants;
using Application.Dtos.StationFeedback.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.StationFeedback
{
    public class StationFeedbackCreateReqValidator : AbstractValidator<StationFeedbackCreateReq>
    {
        public StationFeedbackCreateReqValidator()
        {
            RuleFor(x => x.StationId)
                .NotEqual(Guid.Empty)
                .WithMessage(Message.StationFeedbackMessage.NotFound);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage(Message.StationFeedbackMessage.InvalidRating);

            RuleFor(x => x.content)
                .MaximumLength(1000)
                .WithMessage(Message.StationFeedbackMessage.ContentTooLong)
                .When(x => !string.IsNullOrWhiteSpace(x.content));
        }
    }
}
