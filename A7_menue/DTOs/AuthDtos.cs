using System.ComponentModel.DataAnnotations;

namespace A7_menue.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Cafe name is required.")]
        public string CafeName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email {  get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters .")]
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
