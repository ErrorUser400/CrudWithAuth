using CrudWithAuth.Data;
using CrudWithAuth.Entitites;
using CrudWithAuth.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CrudWithAuth.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        //<summary>
        // log in the user and make sure password hash match with the database
        // if anything goes wrong return null
        // else return access token and refresh token
        //</summary>
        public async Task<TokenResponseDto?> LoginAsync(UserRequestDto request)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PassWordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        //<summary>
        //logout the user by removing and set null the refresh token and refresh token expiry time
        // return null if anything goes wrong
        //</summary>
        public async Task<string?> LogOutAsync(int userId)
        {
            try
            {
                var dbUser = await _context.users.FindAsync(userId);
                dbUser.RefreshToken = null;
                dbUser.RefreshTokenExpiryTime = null;

                await _context.SaveChangesAsync();
            }
            catch (DbException e)
            {
                return null;
            }

            return "success";
        }

        //<summary>
        // register the user. if information passed in already exist in the database return null
        // else hash the password and add the user to the database
        //</summary>
        public async Task<User?> RegisterAsync(UserRequestDto request)
        {
            if (await _context.users.AnyAsync(u => u.UserName == request.UserName))
            {
                return null;
            }

            User user = new();

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.UserName = request.UserName;
            user.PassWordHash = hashedPassword;

            _context.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        //<summary>
        // pass in the RefreshTokenRequestDro and userId to return new accesstoken and potentially new refreshtoken
        //</summary>
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, int userId)
        {
            var user = await ValidateRefreshTokenAsync(userId, request.RefreshToken);

            if (user is null) { return null; }

            return await CreateTokenResponse(user);
        }

        //<summary>
        // use the User passed in return new accesstoken and potentially new refreshtoken
        //</summary>
        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        //<summary>
        //user the Id and refresh token passed to make sure the user exist and the refresh token is valid
        //</summary>
        private async Task<User?> ValidateRefreshTokenAsync(int Id, string refreshToken)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }


        //<summary>
        // generate and save refreshtoken if it doesn't exist.
        // if it exist return an empty string
        //</summary>
        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var sbRefreshtoken = new StringBuilder("");

            if (user.RefreshToken is null)
            {
                sbRefreshtoken.Append(GenerateRefreshToken());
                user.RefreshToken = sbRefreshtoken.ToString();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }

            await _context.SaveChangesAsync();

            return sbRefreshtoken.ToString();
        }

        //<summary>
        // generate refresh token by creating an byte array and fill it with crypographically strong random value 
        //</summary>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        //<summary>
        // Create a token by passing in the user and then create a list of Claims
        // (Name = UserName, NameIdentifier = user.Id, Role = userRole)
        // set up Jwt function by creating a symmentricSecuritykey that goes into SigningCredentials
        // that goes into a JwtSecurityToken before finally creating the token
        //</summary>
        private string CreateToken(User user)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Roles.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var TokenDescriptor = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                    audience: _configuration.GetValue<string>("AppSettings:Audience"),
                    claims: Claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
        }
    }
}
