using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePayment(Payment payment);
    }
}
