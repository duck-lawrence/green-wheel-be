using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Payment.Request
{
    public class PaymentReqValidator : AbstractValidator<PaymentReq>
    {
        public PaymentReqValidator()
        {
            RuleFor(x => x.PaymentMethod)
                .InclusiveBetween(0, 1).WithMessage("payment.invalid_payment_method");

            RuleFor(x => x.FallbackUrl)
                .NotEmpty().WithMessage("payment.fallback_url_required")
                .Must(IsValidUrl).WithMessage("payment.fallback_url_invalid");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("payment.amount_must_be_positive")
                .When(x => x.Amount.HasValue);
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttps || result.Scheme == Uri.UriSchemeHttp);
        }
    }
}
