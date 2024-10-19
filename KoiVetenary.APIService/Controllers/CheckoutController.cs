using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Service.DTO.VNPAY;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace KoiVetenary.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController:ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IVnPayService _vnpayService;
        private readonly IConfiguration _configuration;

        public CheckoutController(ICheckoutService checkoutService, IVnPayService vnpayService, IConfiguration configuration)
        {
            _checkoutService = checkoutService;
            _vnpayService = vnpayService;
            _configuration = configuration;
        }

        [HttpPost("Checkout")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<string> Checkout(int appointmentId)
        {
            var appointment = _checkoutService.Checkout(appointmentId);
            
            
            //var historySubId = historySub.HisSubscriptionId;
            //var amount = historySub.Total;
            var vnPayModel = new VnPayRequestModel
            {
                AppointmentId = appointmentId,
                OwnerId = "Name",
                Notes = "Pay for treatment",
                TotalCost = appointment.Result.TotalCost,
                AppointmentDate = (DateTime)appointment.Result.AppointmentDate,
                TotalEstimatedDuration = appointment.Result.TotalEstimatedDuration,
            };
            return _vnpayService.CreatePaymentUrl(HttpContext, vnPayModel);
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> HandleVnPayReturn([FromQuery] VnPayReturnModel model)
        {
            if (model.Vnp_TransactionStatus != "00") return BadRequest();
            var transaction = new Payment
            {
                AppointmentId = (int)model.Vnp_OrderInfo,
                PaymentDate = DateTime.ParseExact((string)model.Vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                TotalAmount = model.Vnp_Amount,
                TransactionNo = model.Vnp_TransactionNo,
                ResponseCode = model.Vnp_ResponseCode,
                TransactionStatus = model.Vnp_TransactionStatus,
                CreatedAt = DateTime.Now,
                ResponseId = model.Vnp_TransactionStatus,
                TmnCode = model.Vnp_TmnCode,
                TxnRef = model.Vnp_TxnRef,
                Amount = model.Vnp_Amount,
                OrderInfo = "Pay for your fish",
                Message = "Pay at vetenary koi",
                PayDate = DateTime.Now,
                BankCode = model.Vnp_BankCode,
                TransactionType = model.Vnp_CardType,
                SecureHash = model.Vnp_SecureHash,

            };
            var orderId = Convert.ToInt32(model.Vnp_OrderInfo);
            //await _checkoutService.CreateHistory(orderId, transaction);
            //await _checkoutService.CreateSubscription(orderId);
            return Redirect($"{_configuration["VnPay:UrlReturnPayment"]}/{orderId}");
        }
    }
}
