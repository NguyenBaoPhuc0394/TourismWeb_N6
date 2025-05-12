using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TourismWeb.Data;
using TourismWeb.Models;
using TourismWeb.Repositories.Interfaces;

namespace TourismWeb.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TourismDbContext _tourismDbContext;
        private readonly IMapper _mapper;
        public AccountRepository(TourismDbContext tourismDbContext, IMapper mapper) 
        {
            _tourismDbContext = tourismDbContext;
            _mapper = mapper;
        }

        public async Task<Account> CreateAccount(Account account, string otp)
        {
            var result = _tourismDbContext.Accounts
                .FromSqlRaw("EXEC CreateAccount @Username = {0}, @Password = {1}, @Email = {2}, @OTP = {3}",
                    account.Username, account.Password, account.Email, otp)
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
            {
                // Handle the case where no account was created (e.g., OTP invalid, duplicate username)
                throw new InvalidOperationException("Account creation failed. No account was returned.");
            }

            return result;
        }

        public async Task<int> CheckExistUsernameAndEmail(Account account)
        {
            var existingUsername = await _tourismDbContext.Accounts.FirstOrDefaultAsync(x => x.Username == account.Username);
            var existingEmail = await _tourismDbContext.Accounts
                    .FirstOrDefaultAsync(x => x.Email == account.Email);
            if (existingUsername != null) return 1;
            if(existingEmail != null) return 2;
            return 0;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _tourismDbContext.Accounts.ToListAsync();
        }

        public async Task<Account> GetAccountById(string id)
        {
            var account = await _tourismDbContext.Accounts.FindAsync(id);
            if(account == null)
            {
                throw new Exception("Account not found!");
            }
            return account;
        }

        public async Task DeleteAccountById(string id)
        {
            var account = await _tourismDbContext.Accounts.FindAsync(id);
            if(account == null)
            {
                throw new Exception("Account not found!");
            }
            _tourismDbContext.Accounts.Remove(account);
            await _tourismDbContext.SaveChangesAsync();
        }

        public async Task<Account> CheckAccountExists(Account account)
        {
            var existingAccount = await _tourismDbContext.Accounts.FirstOrDefaultAsync(acc => acc.Username == account.Username && acc.Password == account.Password);
            return existingAccount;
        }


        public async Task<bool> UpdatePasswordByEmail(string email, string OldPassword, string NewPassword)
        {
            var existingAccount = await _tourismDbContext.Accounts.FirstOrDefaultAsync(acc => acc.Email == email && acc.Password == OldPassword);

            if (existingAccount == null)
            {
                return false;
            }

            existingAccount.Password = NewPassword;

            await _tourismDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmEmail(Account account)
        {
            var confirmAccount = await _tourismDbContext.Accounts
                .FirstOrDefaultAsync(a => a.Email == account.Email && a.OTP == account.OTP);

            if (confirmAccount == null) return false;

            confirmAccount.isConfirmed = 1;
            confirmAccount.OTP = null;
            _tourismDbContext.Accounts.Update(confirmAccount);
            await _tourismDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetOtp(string email, string otp)
        {
            try
            {
                var account = await _tourismDbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email);
                if (account == null) throw new Exception("Account không tồn tại");
                account.OTP = otp;
                await _tourismDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task ResetPass(Account account)
        {
            try
            {
                var accountToReset = await _tourismDbContext.Accounts.FirstOrDefaultAsync(a => a.Email == account.Email && a.OTP == account.OTP);
                if (accountToReset == null) throw new Exception("Mã Otp sai hoặc Enail không hợp lệ");
                accountToReset.OTP = null;
                accountToReset.Password = account.Password;
                await _tourismDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCusIDByUsername(string username)
        {
            var customerId = _tourismDbContext.Accounts
                .Where(a => a.Username == username)
                .Join(_tourismDbContext.Customers,
                      a => a.Email,
                      c => c.Email,
                      (a, c) => c.Id)
                .FirstOrDefault();

            return customerId;
        }

        public async Task<bool> CheckAdminAccountExist(string username, string password)
        {
            var accountExists = await _tourismDbContext.Accounts
                .AnyAsync(a => a.Username == username && a.Password == password && a.Role == 0);

            return accountExists;
        }

    }
}
