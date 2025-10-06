using Application.Abstractions;
using Application.AppExceptions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Application.Dtos.Momo.Request;
using Application.Dtos.Momo.Respone;
using Application.Helpers;
using Application.Repositories;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace Application
{
    public class MomoService : IMomoService
    {
        private readonly MomoSettings _momoSettings;
        private readonly HttpClient _httpClient;
        private readonly IMomoPaymentLinkRepository _momoPaymentLinkRepositorys;
        private readonly IInvoiceRepository _invoiceRepository;

        public MomoService(IOptions<MomoSettings> momoSettings, HttpClient httpClient, IMomoPaymentLinkRepository momoPaymentLinkRepositorys, IInvoiceRepository invoiceRepository)
        {
            _momoSettings = momoSettings.Value;
            _httpClient = httpClient;
            _momoPaymentLinkRepositorys = momoPaymentLinkRepositorys;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<string> CreatePaymentAsync(decimal amount, Guid invoiceId, string description)
        {
            var paymentLink = await _momoPaymentLinkRepositorys.GetPaymentLinkAsync(invoiceId.ToString());
            if(!string.IsNullOrEmpty(paymentLink))
            {
                return paymentLink;
            }
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if(invoice == null)
            {
                throw new NotFoundException(Message.Invoice.InvoiceNotFound);
            }
            if (invoice.Status == (int)InvoiceStatus.Paid || invoice.Status == (int)InvoiceStatus.Cancelled)
            {
                throw new BadRequestException(Message.Invoice.ThisInvoiceWasPaidOrCancel);
            }
            var requestId = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            
            var rawData =
                    $"accessKey={_momoSettings.AccessKey}" +
                    $"&amount={amount.ToString("0", CultureInfo.InvariantCulture)}" +
                    $"&extraData={""}" +
                    $"&ipnUrl={_momoSettings.IpnUrl}" +
                    $"&orderId={invoiceId}" +
                    $"&orderInfo={description}(Invoice ID: {invoiceId})" +
                    $"&partnerCode={_momoSettings.PartnerCode}" +
                    $"&redirectUrl={_momoSettings.RedirectUrl}" +
                    $"&requestId={requestId}" +
                    $"&requestType={_momoSettings.RequestType}";


            //create signature
            var signature = MomoHelper.CreateSignature(rawData, _momoSettings.SecretKey);

            //Create request send to momo
            var request = new MomoPaymentReq()
            {
                PartnerCode = _momoSettings.PartnerCode,
                RequestId = requestId,
                Amount = amount.ToString("0"),
                OrderId = invoiceId.ToString(),
                OrderInfo = $"{description}(Invoice ID: {invoiceId})",
                RedirectUrl = _momoSettings.RedirectUrl,
                IpnUrl = _momoSettings.IpnUrl,
                RequestType = _momoSettings.RequestType,
                ExtraData = "",
                Lang = _momoSettings.Lang ?? "en",
                Signature = signature
            };

            //send request to momo
            var response = await _httpClient.PostAsJsonAsync(_momoSettings.Endpoint, request);

            if (!response.IsSuccessStatusCode)
            {
                if((int)response.StatusCode == 400)
                {
                    throw new BadRequestException(Message.Momo.InvalidSignature);
                }
                if ((int)response.StatusCode == 401)
                {
                    throw new UnauthorizedAccessException(Message.Momo.MissingAccessKeyPartnerCodeSecretKey);
                }
                if ((int)response.StatusCode == 403)
                {
                    throw new BadRequestException(Message.Momo.NotHavePermission);
                }
                if ((int)response.StatusCode == 404)
                {
                    throw new BadRequestException(Message.Momo.InvalidEndpoint);
                }

            }
            // Deserialize phản hồi JSON
            //Đọc json (respone) từ momo và map vào MomoPaymentRes
            var momoResponse = await response.Content.ReadFromJsonAsync<MomoPaymentRes>();

            
            if (momoResponse == null)
            {
                throw new Exception(Message.Json.ParsingFailed);
            }

            if (momoResponse.ResultCode != 0)
            {
                throw new Exception(Message.Momo.FailedToCreateMomoPayment);
            }
            //save to redis
            await _momoPaymentLinkRepositorys.SavePaymentLinkPAsyns(invoiceId.ToString(), momoResponse.ShortLink);
            return momoResponse.ShortLink;
        }

        public async Task VerifyMomoIpnReq(MomoIpnReq req)
        {
            var rawData =
                $"accessKey={_momoSettings.AccessKey}" +
                $"&amount={req.Amount.ToString("0", CultureInfo.InvariantCulture)}" +
                $"&extraData={(req.ExtraData ?? "")}" +
                $"&message={req.Message}" +
                $"&orderId={req.OrderId}" +
                $"&orderInfo={req.OrderInfo}" +
                $"&orderType={req.OrderType}" +
                $"&partnerCode={req.PartnerCode}" +
                $"&payType={req.PayType}" +
                $"&requestId={req.RequestId}" +
                $"&responseTime={req.ResponseTime}" +
                $"&resultCode={req.ResultCode}" +
                $"&transId={req.TransId}";


            var signature = MomoHelper.CreateSignature(rawData, _momoSettings.SecretKey);

            if (!string.Equals(signature, req.Signature, StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException("Invalid MoMo signature");
            }
           
            
        }
    }
}
