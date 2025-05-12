using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> GetCustomerById(string id);
    }
}
