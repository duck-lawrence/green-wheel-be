using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Application.Dtos.Invoice.Response;
using Application.Dtos.Momo.Request;
using Application.Helpers;
using Application.UnitOfWorks;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Application
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceUow _uow;
        private readonly IMapper _mapper;
        private readonly IMomoService _momoService;
        private readonly IEmailSerivce _emailService;

        public InvoiceService(IInvoiceUow uow, IMapper mapper, IMomoService momoService, IOptions<EmailSettings> emailSettings, IEmailSerivce emailSerivce)
        {
            _uow = uow;
            _mapper = mapper;
            _momoService = momoService;
            _emailService = emailSerivce;
        }

        public async Task PayHandoverInvoiceManual(Invoice invoice, decimal amount)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId)
                ?? throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            //lấy ra số tiền cần thanh toán (trừ cho reservation invoice nếu đã thanh toán)
            var reservationInvoice = (await _uow.InvoiceRepository.GetByContractAsync(invoice.ContractId)).FirstOrDefault(i => i.Type == (int)InvoiceType.Reservation);
            var amountNeed = InvoiceHelper.CalculateTotalAmount(invoice)
                              - (reservationInvoice != null ? reservationInvoice.Status == (int)InvoiceStatus.Paid ? reservationInvoice.Subtotal : 0 : 0);
            if (amount < amountNeed) throw new BusinessException(Message.InvoiceMessage.InvalidAmount);
            await UpdateCashInvoice(invoice, amount);
            await CancleReservationInvoice(invoice);
            contract.Status = (int)RentalContractStatus.Active;
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.SaveChangesAsync();
        }

        public async Task PayReturnInvoiceManual(Invoice invoice, decimal amount)
        {
            var amountNeed = InvoiceHelper.CalculateTotalAmount(invoice);
            if (amount < amountNeed) throw new BusinessException(Message.InvoiceMessage.InvalidAmount);
            await UpdateCashInvoice(invoice, amount);
            await _uow.SaveChangesAsync();
        }
        public async Task PayReservationInvoiceManual(Invoice invoice, decimal amount)
        {
            var contract = await _uow.RentalContractRepository.GetByIdAsync(invoice.ContractId) 
                ?? throw new NotFoundException(Message.RentalContractMessage.RentalContractNotFound);
            var amountNeed = InvoiceHelper.CalculateTotalAmount(invoice);             
            if (amount < amountNeed) throw new BusinessException(Message.InvoiceMessage.InvalidAmount);
            await UpdateCashInvoice(invoice, amount);
            contract.Status = (int)RentalContractStatus.Active;
            await _uow.RentalContractRepository.UpdateAsync(contract);
            await _uow.SaveChangesAsync();
        }
        private async Task UpdateCashInvoice(Invoice invoice, decimal amount)
        {
            invoice.PaidAmount = amount;
            invoice.PaidAt = DateTimeOffset.UtcNow;
            invoice.Status = (int)InvoiceStatus.Paid;
            invoice.PaymentMethod = (int)PaymentMethod.Cash;
            await _uow.InvoiceRepository.UpdateAsync(invoice);
        }


        public async Task<string> PayHandoverInvoiceOnline(Invoice invoice, string fallbackUrl)
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

        public async Task<string> PayReservationInvoiceOnline(Invoice invoice, string fallbackUrl)
        {
            var amount = InvoiceHelper.CalculateTotalAmount(invoice);
            var link = await _momoService.CreatePaymentAsync(amount, invoice.Id, invoice.Notes, fallbackUrl);
            return link;
        }

        public async Task<string> PayReturnInvoiceOnline(Invoice invoice, string fallbackUrl)
        {
            var amount = InvoiceHelper.CalculateTotalAmount(invoice);
            var link = await _momoService.CreatePaymentAsync(amount, invoice.Id, invoice.Notes, fallbackUrl);
            return link;
        }

        public async Task UpdateInvoiceMomoPayment(MomoIpnReq momoIpnReq, Guid invoiceId)
        {
            if (momoIpnReq.ResultCode == (int)MomoPaymentStatus.Success)
            {
                var invoice = await _uow.InvoiceRepository.GetByIdAsync(invoiceId);
                if (invoice == null)
                {
                    throw new NotFoundException(Message.InvoiceMessage.NotFound);
                }
                invoice.Status = (int)InvoiceStatus.Paid;
                invoice.PaymentMethod = (int)PaymentMethod.MomoWallet;
                invoice.PaidAmount = momoIpnReq.Amount;
                invoice.PaidAt = DateTimeOffset.FromUnixTimeMilliseconds(momoIpnReq.ResponseTime);
                await _uow.InvoiceRepository.UpdateAsync(invoice);

                //thanh toán handover thì cancel reservation
                if (invoice.Type == (int)InvoiceType.Handover)
                {
                    await CancleReservationInvoice(invoice);
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
                await _emailService.SendEmailAsync(customer.Email, subject, body);
            }
        }

        public async Task<PageResult<Invoice>> GetAllInvoicesAsync(PaginationParams pagination)
        {
            var invoices = await _uow.InvoiceRepository.GetAllInvoicesAsync(pagination);

            if (invoices == null || !invoices.Items.Any())
                return null;

            return invoices;
        }

        public async Task<InvoiceViewRes> GetInvoiceById(Guid id, bool includeItems = false, bool includeDeposit = false)
        {
            var invoice = await _uow.InvoiceRepository.GetByIdOptionAsync(id, includeItems, includeDeposit);
            if (invoice == null)
            {
                throw new NotFoundException(Message.InvoiceMessage.NotFound);
            }
            var reservationInvoice = (await _uow.InvoiceRepository.GetByContractAsync(invoice.ContractId))
                            .Where(i => i.Type == (int)InvoiceType.Reservation).FirstOrDefault();
            var reservationFee = 0;
            if (reservationInvoice != null && reservationInvoice.Status == (int)InvoiceStatus.Paid)
            {
                reservationFee = (int)reservationInvoice.Subtotal;
            }
            var invoiceViewRes = _mapper.Map<InvoiceViewRes>(invoice, otp => otp.Items["ReservationFee"] = reservationFee);
            return invoiceViewRes;
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
                throw new NotFoundException(Message.InvoiceMessage.NotFound);
            }
            return invoice;
        }

        private async Task CancleReservationInvoice(Invoice handoverInvoice)
        {  
            var reservationInvoice = (await _uow.InvoiceRepository.GetByContractAsync(handoverInvoice.ContractId)).FirstOrDefault(i => i.Type == (int)InvoiceType.Reservation);
            if (reservationInvoice.Status == (int)InvoiceStatus.Pending)
            {
                reservationInvoice.Status = (int)InvoiceStatus.Cancelled;
                await _uow.InvoiceRepository.UpdateAsync(reservationInvoice);
            }
        }
    }
}