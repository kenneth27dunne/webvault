using ClientApp.Models;

namespace ClientApp.Services.Interfaces
{
    internal interface IAuthManager
    {
        Task<User> GetUserFromAuthState();
        Task<bool> Login(LoginViewModel vm);
        Task<bool> CreateUser(RegisterViewModel vm);
    }
}