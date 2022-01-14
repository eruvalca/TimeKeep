using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using TimeKeep.Client.Providers;
using TimeKeep.Client.Utilities;
using TimeKeep.Shared;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Client.Services
{
    public class UsersService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public string ClaimType { get; private set; }

        public UsersService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponse> Register(RegisterVM registerViewModel)
        {
            var response = await _client.PostAsJsonAsync("users/register", registerViewModel);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.IsSuccess)
            {
                if (!String.IsNullOrEmpty(authResult.Token))
                {
                    await _localStorage.SetItemAsync("authToken", authResult.Token);
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.Token);
                    ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
                }
            }

            return authResult;
        }

        public async Task<AuthResponse> Login(LoginVM loginViewModel)
        {
            var response = await _client.PostAsJsonAsync("users/login", loginViewModel);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.IsSuccess)
            {
                await _localStorage.SetItemAsync("authToken", authResult.Token);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.Token);
                ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
            }

            return authResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _client.DefaultRequestHeaders.Authorization = null;
            ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<AuthResponse> UpdateUser(UserVM user)
        {
            var response = await _client.PutAsJsonAsync("users/update", user);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<TimeKeepUser> GetUserDetails()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            var claims = JwtParser.ParseClaimsFromJwt(token);

            var user = new TimeKeepUser
            {
                Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value,
                LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value,
                HireDate = DateTime.Parse(claims.FirstOrDefault(c => c.Type == "HireDate").Value),
                VacationDaysAccruedPerMonth = decimal.Parse(claims.FirstOrDefault(c => c.Type == "VacationDaysAccruedPerMonth").Value),
                SickHoursAccruedPerMonth = decimal.Parse(claims.FirstOrDefault(c => c.Type == "SickHoursAccruedPerMonth").Value),
                PersonalDaysPerYear = int.Parse(claims.FirstOrDefault(c => c.Type == "PersonalDaysPerYear").Value)
            };

            return user;
        }
    }
}
