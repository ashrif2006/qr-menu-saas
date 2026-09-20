namespace A7_menue.DTOs
{
    public class RegisterRequest
    {
        public string CafeName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
    public class LoginRequest
    {
        public string Email {  set; get; } = string.Empty;
        public string Password { get; set; } = string.Empty ;

    }
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string CafName {  get; set; } = string.Empty;

    }
}
