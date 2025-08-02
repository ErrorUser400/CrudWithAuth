using CrudWithAuth.Data;
using CrudWithAuth.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrudWithAuth.Service
{
    public class AuthService : IAuthService
    {
        private IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }
        public async Task<UserModel>? AutheticateUser(UserModelDTO login)
        {
            UserModel? user = await _context.userModels.FirstOrDefaultAsync(u => u.Username == login.Username) ?? null;

            return user;
        }
        public string GenerateJsonWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAdress),
                new Claim("JoinedAt", userInfo.JoinedAt.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
