namespace TourismWeb.DTOs.Account
{
    public class AccountConfirmDTO
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        
        public AccountConfirmDTO(string Email, string otp) { 
            this.Email = Email;
            this.OTP = otp;
        }
    }
}
