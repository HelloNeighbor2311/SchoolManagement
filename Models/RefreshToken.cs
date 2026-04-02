namespace SchoolManagement.Models
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsRevoked { get; set; }
        public DateTime ExpiredDate { get; set; }

        public User? User { get; set; }
    }
}
