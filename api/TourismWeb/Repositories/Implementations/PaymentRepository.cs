using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.DTOs.Payment;
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

        public async Task<Payment> CreatePayment(PaymentCreateDTO data)
        {
            var result = _context.Payments
                .FromSqlRaw("EXEC CreatePayment @BookingId = {0}, @Amount = {1}", data.Booking_Id, data.Amount)
                .AsEnumerable()
                .FirstOrDefault();
            if (result == null)
            {
                throw new Exception("Insert thất bại");
            }
            return result;
        }

        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _context.Payments.ToListAsync();

        }
    }
}
