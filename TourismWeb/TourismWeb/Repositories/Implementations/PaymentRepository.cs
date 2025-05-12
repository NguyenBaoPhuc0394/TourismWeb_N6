using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly TourismDbContext _context;

        public PaymentRepository(TourismDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePayment(Payment payment)
        {
            var result = _context.Payments.FromSqlRaw("EXEC CreatePayment " +
                "@Booking_Id = {0}",
                payment.Booking_Id)
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Schedule not found!");
            }
            return result;
        }
    }
}
