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

        public async Task<PaymentDTO> CreatePayment(PaymentCreateDTO createDTO)
        {
            var payment = _mapper.Map<Payment>(createDTO);
            var addedPayment = await _paymentRepository.CreatePayment(payment);
            return _mapper.Map<PaymentDTO>(addedPayment);
        }
    }
}
