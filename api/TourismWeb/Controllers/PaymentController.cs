using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourismWeb.DTOs.Payment;
using TourismWeb.Services.Interfaces;
using NLog; 

namespace TourismWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); 

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            Logger.Debug("PaymentController initialized.");
        }

        // POST: api/Payment/create
        [HttpPost("create")]
        public async Task<ActionResult<PaymentDTO>> CreatePayment(PaymentCreateDTO createDTO)
        {
            Logger.Info("Payment creation attempt.");
            try
            {
                var result = await _paymentService.CreatePayment(createDTO);
                Logger.Info($"Payment created successfully: {result.Id}");
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                Logger.Warn($"Payment creation failed: {ex.Message}");
                return NotFound(new { status = 404, message = "Không tìm thấy đối tượng!", detail = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error creating payment.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }

        // GET: api/Payment/all-payment
        [HttpGet("all-payment")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetAllPayments()
        {
            Logger.Info("Fetching all payments.");
            try
            {
                var payments = await _paymentService.GetAllPayment();
                Logger.Debug($"Retrieved {payments.Count()} payments.");
                return Ok(payments);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all payments.");
                return StatusCode(500, new { status = 500, message = "Có lỗi xảy ra!", detail = ex.Message });
            }
        }
    }
}