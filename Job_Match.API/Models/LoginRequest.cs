namespace Job_Match.API.Models
{
    public class LoginRequest
    {
        public required string mail { get; set; }
        public required string password { get; set; }
    }
}
