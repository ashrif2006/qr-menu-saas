using A7_menue.Data;
using A7_menue.DTOs;
using A7_menue.Models;

using Microsoft.EntityFrameworkCore;

namespace A7_menue.Services
{
    public class AuthService(AppDbContext context, JwtService jwtService) : IAuthService
    {
        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var emailExists = await context.users.AnyAsync(u => u.Email == request.Email);
            if (emailExists)
            {
                return null;
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

            return new AuthResponse 
            { 
                Token = token,
                FullName = user.UserName,
                CafName = tenant.Name
            };
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await context.users
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.Email==request.Email);
            if(user==null || !BCrypt.Net.BCrypt.Verify(request.Password , user.PasswordHash))
            {
                return null;
            }
            var token = jwtService.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                FullName = user.UserName,
                CafName = user.Tenant.Name
            };
        }

        private string GenerateSlug(string name)
        {
            return name.ToLower()
                .Replace(" ", "-")
                + "-" + Guid.NewGuid().ToString("N")[..6];
        }
    }
}
