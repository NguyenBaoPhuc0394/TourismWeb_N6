using AutoMapper;
using TourismWeb.DTOs.Payment;
using TourismWeb.DTOs.Schedule;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<Payment> CreatePayment(PaymentCreateDTO createDTO)
        {
            try
            {
                var addedPayment = await _paymentRepository.CreatePayment(createDTO);
                return addedPayment;
            } catch (Exception) {
                throw;
            }
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPayment()
        {
            var payments = await _paymentRepository.GetAllPayments();

            return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }

    }
}
