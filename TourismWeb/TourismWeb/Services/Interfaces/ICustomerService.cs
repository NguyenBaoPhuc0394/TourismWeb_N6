using TourismWeb.DTOs.Customer;
using TourismWeb.Models;

namespace TourismWeb.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomer(CustomerCreateDTO createDTO);
        Task<CustomerUpdateDTO> UpdateCustomer(CustomerUpdateDTO updateDTO);
        Task<CustomerDTO> GetCustomerByEmail(string email);

        Task<Customer> GetCustomerById(string id);

    }
}
