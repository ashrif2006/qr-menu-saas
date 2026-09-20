using A7_menue.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace A7_menue.Services
{
    public class JwtService(IConfiguration config)
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("TenantId" , user.TenantId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryMinutes = int.Parse(config["Jwt:ExpiryMinutes"]!);
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience : config["Jwt:Audience"],
                claims : claims,
                expires : DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials : creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token) ;
        }
    }
}
