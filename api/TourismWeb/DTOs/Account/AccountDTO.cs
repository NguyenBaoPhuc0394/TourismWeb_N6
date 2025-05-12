namespace TourismWeb.DTOs.Account
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public int isConfirmed { get; set; }
    }
}
