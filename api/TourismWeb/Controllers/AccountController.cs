using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TourismWeb.DTOs.Account;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;
using NLog; 

namespace TourismWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _config = config;
            Logger.Debug("AccountController initialized.");
        }

        // POST: api/Account/register
        [HttpPost("register")]
        public async Task<ActionResult<AccountRegisterDTO>> CreateAccount(AccountRegisterDTO registerDTO)
        {
            Logger.Info($"Register attempt for username: {registerDTO.Username}, email: {registerDTO.Email}");
            var isExisted = await _accountService.CheckExistEmailAndUsername(registerDTO);
            if (isExisted == 1)
            {
                Logger.Warn($"Registration failed: Username {registerDTO.Username} already exists.");
                return BadRequest(new { message = "Username này đã được sử dụng" });
            }
            if (isExisted == 2)
            {
                Logger.Warn($"Registration failed: Email {registerDTO.Email} already exists.");
                return BadRequest(new { message = "Email này đã được sử dụng" });
            }
            try
            {
                var result = await _accountService.CreateAccount(registerDTO);
                if (result == null)
                {
                    Logger.Error("Account creation failed: Result is null.");
                    return StatusCode(500, new { message = "Tạo tài khoản thất bại." });
                }

                Logger.Info($"Account created successfully for username: {registerDTO.Username}");
                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error creating account for username: {registerDTO.Username}");
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Account/all-accounts
        [HttpGet("all-accounts")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            Logger.Info("Fetching all accounts.");
            try
            {
                var result = await _accountService.GetAllAccounts();
                Logger.Debug($"Retrieved {result.Count()} accounts.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error fetching all accounts.");
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách tài khoản." });
            }
        }

        // GET: api/Account/id
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(string id)
        {
            Logger.Info($"Fetching account with ID: {id}");
            try
            {
                var account = await _accountService.GetAccountById(id);
                if (account == null)
                {
                    Logger.Warn($"Account with ID {id} not found.");
                    return NotFound(new { message = $"Không tìm thấy tài khoản với ID: {id}" });
                }
                Logger.Debug($"Account retrieved: {id}");
                return Ok(account);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching account with ID: {id}");
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin tài khoản." });
            }
        }

        // DELETE: api/Account/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccountById(string id)
        {
            Logger.Info($"Attempting to delete account with ID: {id}");
            try
            {
                await _accountService.DeleteAccountById(id);
                Logger.Info($"Account with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting account with ID: {id}");
                return StatusCode(500, new { message = "Lỗi khi xóa tài khoản." });
            }
        }

        private string GenerateJwtToken(string username)
        {
            Logger.Debug($"Generating JWT token for username: {username}");
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Logger.Debug($"JWT token generated for username: {username}");
            return tokenString;
        }

        // POST: api/Account/login
        [HttpPost("login")]
        public async Task<ActionResult> CheckAccountExists([FromBody] AccountLoginDTO loginDTO)
        {
            Logger.Info($"Login attempt for username: {loginDTO.Username}");
            var account = await _accountService.CheckAccountExists(loginDTO);

            if (account != null)
            {
                if (account.isConfirmed == 0)
                {
                    Logger.Warn($"Login failed for {loginDTO.Username}: Account not confirmed.");
                    return BadRequest(new { message = "Tài khoản chưa được xác nhận! Vui lòng kiểm tra hộp thư của email đăng ký!" });
                }

                if (account.Role == 0)
                {
                    Logger.Warn($"Login failed for {loginDTO.Username}: Invalid credentials.");
                    return BadRequest(new { message = "Username hoặc Password không chính xác!" });
                }

                var token = GenerateJwtToken(loginDTO.Username);
                Logger.Info($"Login successful for username: {loginDTO.Username}");
                return Ok(new
                {
                    token,
                    account,
                });
            }
            else
            {
                Logger.Warn($"Login failed: Invalid username or password for {loginDTO.Username}");
                return BadRequest(new { message = "Username hoặc Password không chính xác!" });
            }
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string otp)
        {
            Logger.Info($"Email confirmation attempt for email: {email}");
            try
            {
                var result = await _accountService.ConfirmEmail(new AccountConfirmDTO(email, otp));
                if (!result)
                {
                    Logger.Warn($"Email confirmation failed for email: {email}");
                    return BadRequest(new { message = "Xác thực không thành công. Email hoặc OTP không đúng." });
                }

                Logger.Info($"Email {email} confirmed successfully.");
                return Ok(new { message = "Xác thực email thành công." });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error confirming email for: {email}");
                return StatusCode(500, new { message = "Lỗi khi xác thực email." });
            }
        }

        [HttpPut("update-password")]
        public async Task<ActionResult<bool>> UpdatePasswordByEmail(AccountUpdatePasswordDTO updatePasswordDTO)
        {
            Logger.Info($"Password update attempt for email: {updatePasswordDTO.Email}");
            try
            {
                var result = await _accountService.UpdatePasswordByEmail(updatePasswordDTO);
                Logger.Info($"Password updated successfully for email: {updatePasswordDTO.Email}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating password for email: {updatePasswordDTO.Email}");
                return StatusCode(500, new { message = "Lỗi khi cập nhật mật khẩu." });
            }
        }

        [HttpGet("CheckToken")]
        public IActionResult CheckToken()
        {
            Logger.Info("Checking JWT token validity.");
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                Logger.Warn("Token check failed: No valid token provided.");
                return BadRequest(new { message = "Token không tồn tại hoặc không hợp lệ." });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    Logger.Warn("Token check failed: Invalid token format.");
                    return BadRequest(new { message = "Token không hợp lệ." });
                }

                var exp = jwtToken.ValidTo;
                var now = DateTime.UtcNow;

                if (exp < now)
                {
                    Logger.Warn("Token check failed: Token has expired.");
                    return Unauthorized(new { message = "Token đã hết hạn." });
                }

                Logger.Info("Token is valid.");
                return Ok(new
                {
                    message = "Token còn hiệu lực.",
                    expiresAt = exp
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error checking token validity.");
                return BadRequest(new { message = "Token không hợp lệ.", error = ex.Message });
            }
        }

        [HttpPost("SendOTPResetPass")]
        public async Task<IActionResult> SendOTPResetPass([FromBody] string email)
        {
            Logger.Info($"Sending OTP for password reset to email: {email}");
            try
            {
                var result = await _accountService.SendOTP(email);
                Logger.Info($"OTP sent successfully to email: {email}");
                return Ok(new { message = "Gửi OTP thành công." });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error sending OTP to email: {email}");
                return BadRequest(new { message = "Đã có lỗi xảy ra", error = ex.Message });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPass(AccountResetpassDTO accountResetpassDTO)
        {
            Logger.Info($"Password reset attempt for email: {accountResetpassDTO.Email}");
            try
            {
                await _accountService.ResetPassword(accountResetpassDTO);
                Logger.Info($"Password reset successful for email: {accountResetpassDTO.Email}");
                return Ok(new { message = "Mật khẩu được đổi thành công" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error resetting password for email: {accountResetpassDTO.Email}");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login-admin")]
        public async Task<ActionResult> LoginAdmin([FromBody] AccountLoginDTO loginDTO)
        {
            Logger.Info($"Admin login attempt for username: {loginDTO.Username}");
            try
            {
                var result = await _accountService.CheckAdminAccountExist(loginDTO.Username, loginDTO.Password);
                if (!result)
                {
                    Logger.Warn($"Admin login failed for username: {loginDTO.Username}");
                    return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng, hoặc không phải tài khoản admin." });
                }

                Logger.Info($"Admin login successful for username: {loginDTO.Username}");
                return Ok(new { message = "Đăng nhập thành công" });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error during admin login for username: {loginDTO.Username}");
                return StatusCode(500, new { message = "Lỗi khi đăng nhập admin." });
            }
        }
    }
}