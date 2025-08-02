using CrudWithAuth.Model;

namespace CrudWithAuth.Service
{
    public interface IAuthService
    {
        public Task<UserModel> AutheticateUser(UserModelDTO login);
        public string GenerateJsonWebToken(UserModel userInfo);
    }
}
