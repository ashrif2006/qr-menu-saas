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
    public class AuthController(AppDbContext context , JwtService jwtService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var emailExists = await context.users.AnyAsync(u => u.Email == request.Email);
            if (emailExists)
            {
                return BadRequest("Email is already registred");
            }
            var tenant = new Tenant
            {
                Name = request.CafeName,
                Slug = GenerateSlug(request.CafeName),
            };
            context.Tenants.Add(tenant);
            await context.SaveChangesAsync();

            var user = new User
            {
                TenantId = tenant.Id,
                UserName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            context.users.Add(user);
            await context.SaveChangesAsync();
            var token = jwtService.GenerateToken(user);
            return Ok(new AuthResponse
            {
                Token = token,
                FullName = request.FullName,
                CafName = request.CafeName
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await context.users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if(user == null || BCrypt.Net.BCrypt.Verify(request.Password , user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }
            var token = jwtService.GenerateToken(user);
            return Ok(new AuthResponse
            {
                Token = token,
                FullName = user.UserName,
                CafName = user.UserName
            });
        }


        private string GenerateSlug(string name)
        {
            return name.ToLower()
                .Replace(" ", "-")
                + "-" + Guid.NewGuid().ToString("N")[..6]; 
        }
    }
}
