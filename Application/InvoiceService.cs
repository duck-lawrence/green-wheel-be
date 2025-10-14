using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Invoice.Response;
using Application.Dtos.Momo.Request;
using Application.Helpers;
using Application.Repositories;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Application
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceUow _uow;
        private readonly IMapper _mapper;
        private readonly IMomoService _momoService;
        private readonly EmailSettings _emailSettings;

        public InvoiceService(IInvoiceUow uow, IMapper mapper, IMomoService momoService, IOptions<EmailSettings> emailSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _momoService = momoService;
            _emailSettings = emailSettings.Value;
        }

        public async Task CashPayment(Invoice invoice)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId);
            if (contract == null)
            {
                throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            }
            if (contract.Status != (int)RentalContractStatus.PaymentPending)
            {
                throw new BadRequestException(Message.RentalContractMessage.ThisRentalContractAlreadyProcess);
            }
            contract.Status = (int)RentalContractStatus.Active;
            invoice.PaymentMethod = (int)PaymentMethod.Cash;
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.InvoiceRepository.UpdateAsync(invoice);
            await _uow.SaveChangesAsync();
        }

        public async Task<InvoiceViewRes> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false)
        {
            var invoice = await _uow.InvoiceRepository.GetByIdOptionAsync(id, includeItems, includeDeposit);
            if (invoice == null)
            {
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
            }
            var invoiceViewRes = _mapper.Map<InvoiceViewRes>(invoice);
            return invoiceViewRes;
        }

        public async Task<string> ProcessHandoverInvoice(Invoice invoice, string fallbackUrl)
        {
            Invoice reservationInvoice = null;
            reservationInvoice = (await _uow.InvoiceRepository.GetByContractAsync(invoice.ContractId)).FirstOrDefault(i => i.Type == (int)InvoiceType.Reservation);
            var amount = InvoiceHelper.CalculateTotalAmount(invoice)
                              - (reservationInvoice != null ? reservationInvoice.Status == (int)InvoiceStatus.Paid ? reservationInvoice.Subtotal : 0 : 0);
            //nếu mà reservation k null thì ktra coi status của nó có thanh toán hay chưa nếu rồi thì lấy ra phí thay
            //toán còn không thì là 0 cho mọi case
            var link = await _momoService.CreatePaymentAsync(amount, invoice.Id, invoice.Notes, fallbackUrl);
            return link;
        }

        public async Task<string> ProcessReservationInvoice(Invoice invoice, string fallbackUrl)
        {
            var amount = InvoiceHelper.CalculateTotalAmount(invoice);
            var link = await _momoService.CreatePaymentAsync(amount, invoice.Id, invoice.Notes, fallbackUrl);
            return link;
        }

        public async Task<string> ProcessReturnInvoice(Invoice invoice, string fallbackUrl)
        {
            var link = await _momoService.CreatePaymentAsync(invoice.Subtotal, invoice.Id, invoice.Notes, fallbackUrl);
            return link;
        }

        public async Task UpdateInvoiceMomoPayment(MomoIpnReq momoIpnReq, Guid invoiceId)
        {
            if (momoIpnReq.ResultCode == (int)MomoPaymentStatus.Success)
            {
                var invoice = await _uow.InvoiceRepository.GetByIdAsync(invoiceId);
                if (invoice == null)
                {
                    throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
                }
                invoice.Status = (int)InvoiceStatus.Paid;
                invoice.PaymentMethod = (int)PaymentMethod.MomoWallet;
                invoice.PaidAmount = momoIpnReq.Amount;
                invoice.PaidAt = DateTimeOffset.FromUnixTimeMilliseconds(momoIpnReq.ResponseTime);
                await _uow.InvoiceRepository.UpdateAsync(invoice);
                
                if(invoice.Type == (int)InvoiceType.Handover)
                {
                    var reservationInvoice = (await _uow.InvoiceRepository.GetByContractAsync(invoice.ContractId)).FirstOrDefault(i => i.Type == (int)InvoiceType.Reservation);
                    if (reservationInvoice.Status != (int)InvoiceStatus.Paid)
                    {
                        reservationInvoice.Status = (int)InvoiceStatus.Cancelled;
                        await _uow.InvoiceRepository.UpdateAsync(reservationInvoice);
                    }
                }

                await _uow.MomoPaymentLinkRepository.RemovePaymentLinkAsync(invoiceId.ToString());
                await _uow.SaveChangesAsync();
                var subject = "[GreenWheel] Confirm Your Booking by Completing Payment";
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "PaymentSuccessTemplate.html");
                var body = System.IO.File.ReadAllText(templatePath);
                var customer = (await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId))!.Customer;
                body = body
                     .Replace("{CustomerName}", $"{customer.LastName} {customer.FirstName}")
                     .Replace("{InvoiceCode}", invoice.Id.ToString())
                     .Replace("{PaidAmount}", $"{invoice.PaidAmount?.ToString("N0")} VND")
                     .Replace("{PaymentMethod}", Enum.GetName(typeof(PaymentMethod), invoice.PaymentMethod))
                     .Replace("{InvoiceType}", Enum.GetName(typeof(InvoiceType), invoice.Type))
                     .Replace("{PaidAt}", invoice.PaidAt?.ToString("dd/MM/yyyy HH:mm"));
                await EmailHelper.SendEmailAsync(_emailSettings, customer.Email, subject, body);
            }
        }

        public async Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination)
        {
            var invoices = await _uow.InvoiceRepository.GetAllInvoicesAsync(pagination);

            if (invoices == null || !invoices.Items.Any())
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);

            return invoices;
        }

        public async Task<IEnumerable<InvoiceViewRes>?> GetByContractIdAndStatus(Guid? contractId, int? status)
        {
            var invoices = await _uow.InvoiceRepository.GetInvoiceByContractIdAndStatus(contractId, status);
            return _mapper.Map<IEnumerable<InvoiceViewRes>?>(invoices);
        }

        public async Task<Invoice> GetRawInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false)
        {
            var invoice = await _uow.InvoiceRepository.GetByIdOptionAsync(id, includeItems, includeDeposit);
            if (invoice == null)
            {
                throw new NotFoundException(Message.InvoiceMessage.InvoiceNotFound);
            }
            return invoice;
        }
    }
}