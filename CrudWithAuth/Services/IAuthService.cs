using CrudWithAuth.Entitites;
using CrudWithAuth.Model.DTO;

namespace CrudWithAuth.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserRequestDto request);
        Task<TokenResponseDto?> LoginAsync(UserRequestDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, int userId);
        Task<string> LogOutAsync(int userId);
    }
}
