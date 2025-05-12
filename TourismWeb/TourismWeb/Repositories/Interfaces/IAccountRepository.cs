using TourismWeb.DTOs;
using TourismWeb.Models;

namespace TourismWeb.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> CreateAccount(Account account, string otp);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account> GetAccountById(string id);
        Task DeleteAccountById(string id);
        Task<Account> CheckAccountExists(Account account);
        Task<bool> UpdatePasswordByEmail(string email, string OldPassword, string NewPassword);
        Task<int> CheckExistUsernameAndEmail(Account account);
        Task<bool> ConfirmEmail(Account account);
        Task<bool> SetOtp(string email, string otp);
        Task ResetPass(Account account);

        string GetCusIDByUsername(string username);
    }
}
