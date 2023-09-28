using ClientApp.Models;
using Firebase.Auth;
using FirebaseAdmin.Auth;

namespace ClientApp.Services.Interfaces
{
    internal interface IExternalAuthProvider
    {
        Task<AuthResponse> Login(string username, string password);
        Task<AuthResponse> CreateUser(RegisterViewModel vm);
    }
}