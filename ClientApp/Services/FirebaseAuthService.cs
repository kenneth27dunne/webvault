using ClientApp.Models;
using Firebase.Auth;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
using FirebaseAdmin.Auth;
using System.Security.Claims;
using AutoMapper;
using ClientApp.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientApp.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private FirebaseAuthProvider _authProvider;
        private readonly FirebaseAuth _auth;
        private readonly IMapper _mapper;

        public FirebaseAuthService(string apiKey, IMapper mapper)
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));            
            _mapper = mapper;
            _auth = FirebaseAuth.DefaultInstance;                        
        }

        public async Task<UserRecord> CreateUser(RegisterViewModel vm)
        {
            try
            {
                var request = new UserRecordArgs()
                {
                    Email = vm.EmailAddress,
                    EmailVerified = false,
                    Password = vm.Password,
                    DisplayName = vm.Username,
                    Disabled = false
                };
                var userRecord = await _auth.CreateUserAsync(request);

                var claims = new Dictionary<string, object>()
                {
                    { ClaimTypes.Role, "basic" } // this string can be anything we want ("devAdmin", "Premium", "Basic" etc..)
                };
                await _auth.SetCustomUserClaimsAsync(userRecord.Uid, claims);

                return userRecord;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<UserRecord> GetUserByEmailAsync(string uid)
        {
            try
            {
                var userRecord = await _auth.GetUserByEmailAsync(uid);
                return userRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserRecord> UpdateUser(string uid, UserRecordArgs request)
        {
            request.Uid = uid;
            var userRecord = await _auth.UpdateUserAsync(request);
            return userRecord;
        }

        public async Task SendPasswordResetEmail(string email)
        {
            await _authProvider.SendPasswordResetEmailAsync(email);
        }

        public async Task<AuthResponse> SignInWithEmailAndPasswordAsync(string username, string password)
        {
            try
            {
                var result = await _authProvider.SignInWithEmailAndPasswordAsync(username, password);
                return _mapper.Map<AuthResponse>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public static class FirebaseAuthServiceExtensions
    {
        public static IServiceCollection AddFirebaseAuth(this IServiceCollection services, string apiKey)
        {
            services.AddScoped<IFirebaseAuthService>(_ => new FirebaseAuthService(apiKey, _.GetRequiredService<IMapper>()));
            return services;
        }
    }
}
