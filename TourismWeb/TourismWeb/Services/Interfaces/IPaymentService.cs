using TourismWeb.DTOs.Payment;

namespace TourismWeb.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDTO> CreatePayment(PaymentCreateDTO createDTO);
    }
}
