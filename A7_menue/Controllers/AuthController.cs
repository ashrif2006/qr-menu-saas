using A7_menue.Data;
using A7_menue.DTOs;
using A7_menue.Models;
using A7_menue.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace A7_menue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
           var result = await authService.RegisterAsync(request);
            if(result == null)
            {
                return BadRequest("Email is already registred");
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var reslut = await authService.LoginAsync(request);
            if (reslut == null) 
            {
                return Unauthorized("Invalid Email Or Password ");
            }
            return Ok(reslut);
        }


        private string GenerateSlug(string name)
        {
            return name.ToLower()
                .Replace(" ", "-")
                + "-" + Guid.NewGuid().ToString("N")[..6]; 
        }
    }
}
