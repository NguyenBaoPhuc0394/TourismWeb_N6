using TourismWeb.DTOs.Payment;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePayment(PaymentCreateDTO data);
        Task<IEnumerable<Payment>> GetAllPayments();

    }
}
