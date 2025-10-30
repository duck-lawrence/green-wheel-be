using Application.Constants;
using Application.Dtos.Invoice.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Validators.Invoice
{
    public class UpdateInvoiceReqValidator : AbstractValidator<UpdateInvoiceReq>
    {
        public UpdateInvoiceReqValidator()
        {
            RuleFor(x => x.PaymentMethod)
                .InclusiveBetween(0, 1).WithMessage(PaymentMessage.InvalidPaymentMethod);

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage(InvoiceMessage.AmountRequired);
        }
    }
}
