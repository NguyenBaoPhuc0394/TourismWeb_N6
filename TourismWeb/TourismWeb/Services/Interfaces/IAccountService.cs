using TourismWeb.DTOs.Account;
using TourismWeb.Models;

namespace TourismWeb.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountRegisterDTO> CreateAccount(AccountRegisterDTO registerDTO);
        Task<IEnumerable<AccountDTO>> GetAllAccounts();
        Task<AccountDTO> GetAccountById(string id);
        Task DeleteAccountById(string id);
        Task<AccountDTO> CheckAccountExists(AccountLoginDTO loginDTO);
        Task<bool> UpdatePasswordByEmail(AccountUpdatePasswordDTO updatePasswordDTO);

        Task<int> CheckExistEmailAndUsername(AccountRegisterDTO registerDTO);
        Task<bool> ConfirmEmail(AccountConfirmDTO accountConfirmDTO);
        Task<bool> SendOTP(string email);

        Task ResetPassword(AccountResetpassDTO accountResetpassDTO);
        string GetCusByUsername(string username);
    }
}
