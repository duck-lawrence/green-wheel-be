using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Request
{
    public class MomoPaymentReqValidator : AbstractValidator<MomoPaymentReq>
    {
        public MomoPaymentReqValidator()
        {
            RuleFor(x => x.PartnerCode).NotEmpty().WithMessage("momo.partner_code_required");
            RuleFor(x => x.RequestId).NotEmpty().WithMessage("momo.request_id_required");
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("momo.amount_required")
                .Must(BeValidAmount).WithMessage("momo.amount_invalid");

            RuleFor(x => x.OrderId).NotEmpty().WithMessage("momo.order_id_required");
            RuleFor(x => x.OrderInfo).NotEmpty().WithMessage("momo.order_info_required");

            RuleFor(x => x.RedirectUrl)
                .NotEmpty().WithMessage("momo.redirect_url_required")
                .Must(IsValidUrl).WithMessage("momo.redirect_url_invalid");

            RuleFor(x => x.IpnUrl)
                .NotEmpty().WithMessage("momo.ipn_url_required")
                .Must(IsValidUrl).WithMessage("momo.ipn_url_invalid");

            RuleFor(x => x.Signature).NotEmpty().WithMessage("momo.signature_required");
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttps || result.Scheme == Uri.UriSchemeHttp);
        }

        private bool BeValidAmount(string value)
        {
            return decimal.TryParse(value, out var result) && result > 0;
        }
    }
}
