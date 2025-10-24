using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Request
{
    public class CreateMomoPaymentReqValidator : AbstractValidator<CreateMomoPaymentReq>
    {
        public CreateMomoPaymentReqValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEqual(Guid.Empty).WithMessage("momo.invoice_id_required");

            RuleFor(x => x.FallbackUrl)
                .NotEmpty().WithMessage("momo.fallback_url_required")
                .Must(IsValidUrl).WithMessage("momo.fallback_url_invalid");
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttps || result.Scheme == Uri.UriSchemeHttp);
        }
    }
}
