using TourismWeb.DTOs.Payment;
using TourismWeb.Models;

namespace TourismWeb.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(PaymentCreateDTO createDTO);

        Task<IEnumerable<PaymentDTO>> GetAllPayment();
    }
}
