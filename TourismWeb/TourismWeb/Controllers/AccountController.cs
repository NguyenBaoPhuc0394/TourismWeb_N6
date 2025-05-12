using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using TourismWeb.DTOs.Account;
using TourismWeb.Models;
using TourismWeb.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace TourismWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;
        public AccountController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _config = config;
        }

        // POST: api/Account/register
        [HttpPost("register")]
        public async Task<ActionResult<AccountRegisterDTO>> CreateAccount(AccountRegisterDTO registerDTO)
        {
            var isExisted = await _accountService.CheckExistEmailAndUsername(registerDTO);
            if(isExisted == 1)
                return BadRequest(new { message = "Username này đã được sử dụng" });
            if (isExisted == 2)
                return BadRequest(new { message = "Email này đã được sử dụng" });
            try
            {
                var result = await _accountService.CreateAccount(registerDTO);
                if (result == null)
                    return StatusCode(500, new { message = "Tạo tài khoản thất bại." });

                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Account
        [HttpGet("all-accounts")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            var result = await _accountService.GetAllAccounts();
            return Ok(result);
        }

        // GET: api/Account/id
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(string id)
        {
            var account = await _accountService.GetAccountById(id);
            return Ok(account);
        }

        // DELETE: api/Account/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccountById(string id)
        {
            await _accountService.DeleteAccountById(id);
            return NoContent();
        }

        private string GenerateJwtToken(string username)
        {
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
        // POST: api/Account/login
        [HttpPost("login")]
        public async Task<ActionResult> CheckAccountExists([FromBody] AccountLoginDTO loginDTO)
        {
            var account = await _accountService.CheckAccountExists(loginDTO);

            if (account != null)
            {
                if (account.isConfirmed == 0)
                {
                    return BadRequest(new { message = "Tài khoản chưa được xác nhận! Vui lòng kiểm tra hộp thư của email đăng ký!" });
                }

                if (account.Role == 0)
                {
                    return BadRequest(new { message = "Username hoặc Password không chính xác!" });
                }

                var token = GenerateJwtToken(loginDTO.Username);
                return Ok(new
                {
                    token,
                    account,
                });
            }
            else
            {
                return BadRequest(new { message = "Username hoặc Password không chính xác!" });
            }
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string otp)
        {

            var result = await _accountService.ConfirmEmail(new AccountConfirmDTO(email, otp));

            if (!result)
                return BadRequest(new { message = "Xác thực không thành công. Email hoặc OTP không đúng." });

            return Ok(new { message = "Xác thực email thành công." });
        }

        [HttpPut("update-password")]
        public async Task<ActionResult<bool>> UpdatePasswordByEmail(AccountUpdatePasswordDTO updatePasswordDTO)
        {
            return await _accountService.UpdatePasswordByEmail(updatePasswordDTO);
        }

        [HttpGet("CheckToken")]
        public IActionResult CheckToken()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
                return BadRequest(new { message = "Token không tồn tại hoặc không hợp lệ." });

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return BadRequest(new { message = "Token không hợp lệ." });

                var exp = jwtToken.ValidTo;
                var now = DateTime.UtcNow;

                if (exp < now)
                    return Unauthorized(new { message = "Token đã hết hạn." });

                return Ok(new
                {
                    message = "Token còn hiệu lực.",
                    expiresAt = exp
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Token không hợp lệ.", error = ex.Message });
            }
        }

        [HttpPost("SendOTPResetPass")]
        public async Task<IActionResult> SendOTPResetPass([FromBody] string email)
        {
            try
            {
                var result = await _accountService.SendOTP(email);
                return Ok(new { message = "Gửi OTP thành công." });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã có lỗi xảy ra", error = ex.Message});
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPass(AccountResetpassDTO accountResetpassDTO)
        {
            try
            {
                await _accountService.ResetPassword(accountResetpassDTO);
                return Ok(new { message = "Mật khẩu được đổi thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}