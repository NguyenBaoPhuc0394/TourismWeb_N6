using AutoMapper;
using TourismWeb.DTOs.Customer;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;

namespace TourismWeb.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Customer> CreateCustomer(CustomerCreateDTO createDTO)
        {
            var customer = _mapper.Map<Customer>(createDTO);
            var addedCustomer =  await _customerRepository.CreateCustomer(customer);
            return addedCustomer;
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            return customer;
        }

        public async Task<CustomerUpdateDTO> UpdateCustomer(CustomerUpdateDTO updateDTO)
        {
            var customer = _mapper.Map<Customer>(updateDTO);
            var updatedCustomer = await _customerRepository.UpdateCustomer(customer);
            return _mapper.Map<CustomerUpdateDTO>(updatedCustomer);
        }

        public async Task<CustomerDTO> GetCustomerByEmail(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmail(email);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }
    }
}
