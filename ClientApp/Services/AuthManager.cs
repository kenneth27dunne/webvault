using AutoMapper;
using ClientApp.Models;
using ClientApp.Models.FirestoreModels;
using ClientApp.Services.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components;

namespace ClientApp.Services
{
	internal class AuthManager : IAuthManager
	{
		private readonly IFirebaseAuthService _auth;
        private readonly IFirestoreService _firebase;

        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly IMapper _mapper;
        private readonly NavigationManager _navigationManager;

        public AuthManager(IFirebaseAuthService firebaseManager, CustomAuthenticationStateProvider customAuthenticationStateProvider, 
            IMapper mapper, NavigationManager navigationManager, IFirestoreService firebase)
        {
            _firebase = firebase;
            _mapper = mapper;
            _navigationManager = navigationManager;
            _auth = firebaseManager;
			_customAuthenticationStateProvider = customAuthenticationStateProvider;
		}

        public async Task<Models.User> GetUserFromAuthState()
        {
            try
            {
                var authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    var uid = authState.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                    var userRecord = await _auth.GetUserByEmailAsync(uid);
                    var firestoreUser = await _firebase.GetDocument<FirestoreUser>("UserData", userRecord.Uid);

                    var userResult = _mapper.Map<UserRecord, Models.User>(userRecord);
                    _mapper.Map<FirestoreUser, Models.User>(firestoreUser, userResult);
                    return userResult;
                }

                return new Models.User();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Login(LoginViewModel vm)
		{
			try
			{
                var f = await _auth.SignInWithEmailAndPasswordAsync(vm.EmailAddress, vm.Password);		
                await _customAuthenticationStateProvider.Login(f.Token);
                return true;
            }
			catch (Exception ex)
            {
                // handle errors
                return false;
			}
        }

        public async Task<bool> CreateUser(RegisterViewModel vm)
        {
            try
            {
                var newUser = await _auth.CreateUser(vm);

                //TODO: ADD USERS ADDITIONAL DATA TO USER COLLECTION
                var user = new FirestoreUser()
                {
                    Username = vm.Username,
                    CanContact =  vm.CanContact,
                    FirstName = vm.FirstName,
                    LastName =  vm.LastName                    
                };
                var success = await _firebase.WriteDocument<FirestoreUser>("UserData", newUser.Uid, user);

                var f = await _auth.SignInWithEmailAndPasswordAsync(vm.EmailAddress, vm.Password);
                await _customAuthenticationStateProvider.Login(f.Token);
                return true;
            }
            catch (Exception ex)
            {
                // handle errors
                return false;
            }
        }
    }
}
