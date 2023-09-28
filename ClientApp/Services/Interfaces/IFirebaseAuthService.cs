using ClientApp.Models;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientApp.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        Task<UserRecord> CreateUser(RegisterViewModel vm);
        Task<UserRecord> GetUserByEmailAsync(string uid);
        Task SendPasswordResetEmail(string email);
        Task<AuthResponse> SignInWithEmailAndPasswordAsync(string username, string password);
        Task<UserRecord> UpdateUser(string uid, UserRecordArgs request);
    }
}