using AutoMapper;
using TourismWeb.DTOs.Account;
using TourismWeb.Helpers;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace TourismWeb.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepository accountRepository, IMapper mapper) 
        { 
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountRegisterDTO> CreateAccount(AccountRegisterDTO registerDTO)
        {
            try
            {
                string otp = RandomGenerationHelper.Instance.GenerateRandomOtp();
                var account = _mapper.Map<Account>(registerDTO);
                var addedAccount = await _accountRepository.CreateAccount(account, otp);
                if (addedAccount == null)
                    throw new Exception("Tạo tài khoản thất bại.");
                
                bool sendResult = await EmailSendingHelper.Instance.SendConfirmationEmailAsync(addedAccount.Email, addedAccount.OTP);
                if (!sendResult)
                    throw new Exception("Gửi email xác nhận thất bại.");

                return _mapper.Map<AccountRegisterDTO>(addedAccount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            var result = await _accountRepository.GetAllAccounts();
            return _mapper.Map<IEnumerable<AccountDTO>>(result);
        }

        public async Task<int> CheckExistEmailAndUsername(AccountRegisterDTO registerDTO)
        {
            var account = _mapper.Map<Account>(registerDTO);
            var result = await _accountRepository.CheckExistUsernameAndEmail(account);
            return result;
        }

        public async Task<AccountDTO> GetAccountById(string id)
        {
            var account = await _accountRepository.GetAccountById(id);
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task DeleteAccountById(string id)
        {
            await _accountRepository.DeleteAccountById(id);
        }

        public async Task<AccountDTO> CheckAccountExists(AccountLoginDTO loginDTO)
        {
            var account = _mapper.Map<Account>(loginDTO);
            var existingAccount = await _accountRepository.CheckAccountExists(account);
            return _mapper.Map<AccountDTO>(existingAccount);
        }

        public async Task<bool> UpdatePasswordByEmail(AccountUpdatePasswordDTO updatePasswordDTO)
        {
            return await _accountRepository.UpdatePasswordByEmail(updatePasswordDTO.Email, updatePasswordDTO.OldPassword, updatePasswordDTO.NewPassword);
        }

        public async Task<bool> ConfirmEmail(AccountConfirmDTO accountConfirmDTO)
        {
            var account = _mapper.Map<Account>(accountConfirmDTO);
            return await _accountRepository.ConfirmEmail(account);
        }

        public async Task<bool> SendOTP(string email)
        {
            try
            {
                string otp = RandomGenerationHelper.Instance.GenerateRandomOtp();
                var result = await _accountRepository.SetOtp(email, otp);
                bool sendResult = await EmailSendingHelper.Instance.SendOTP(email, otp);
                if (!sendResult)
                    throw new Exception("Gửi email xác nhận thất bại.");
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ResetPassword(AccountResetpassDTO accountResetpassDTO)
        {
            try
            {
                var account = _mapper.Map<Account>(accountResetpassDTO);
                await _accountRepository.ResetPass(account);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCusByUsername(string username)
        {
            return _accountRepository.GetCusIDByUsername(username);
        }

        public async Task<bool> CheckAdminAccountExist(string username, string password)
        {
            return await _accountRepository.CheckAdminAccountExist(username, password);
        }

    }
}
