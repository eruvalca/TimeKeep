using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimeKeep.Shared;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.ViewModels;
using TimeKeep.Shared.Enums;

namespace TimeKeep.Server.Services
{
    public class UsersService
    {
        private readonly UserManager<TimeKeepUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SymmetricKeyService _symmetricKeyService;
        private readonly PTOEntriesService _ptoEntriesService;
        private readonly IWebHostEnvironment _env;

        public UsersService(UserManager<TimeKeepUser> userManager, IConfiguration configuration, SymmetricKeyService symmetricKeyService, PTOEntriesService ptoEntriesService, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _configuration = configuration;
            _symmetricKeyService = symmetricKeyService;
            _ptoEntriesService = ptoEntriesService;
            _env = env;
        }

        public async Task<AuthResponse> Register(RegisterVM model)
        {
            var user = new TimeKeepUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                HireDate = model.HireDate.ToUniversalTime(),
                VacationDaysAccruedPerMonth = model.VacationDaysAccruedPerMonth,
                SickHoursAccruedPerMonth = model.SickHoursAccruedPerMonth,
                PersonalDaysPerYear = model.PersonalDaysPerYear
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "General");

                if (model.VacationDaysCarriedOver > 0)
                {
                    var ptoEntry = new PTOEntry
                    {
                        PTOHours = (model.VacationDaysCarriedOver * 8) * -1,
                        PTOType = PTOType.VacationCarriedOver,
                        PTODate = new DateTime(DateTime.Today.Year, 1, 1).ToUniversalTime(),
                        CreateDate = DateTime.UtcNow,
                        TimeKeepUserId = user.Id
                    };

                    ptoEntry = await _ptoEntriesService.CreatePTOEntry(ptoEntry);
                }

                return new AuthResponse
                {
                    IsSuccess = true,
                    Messages = new List<string> { "User registered successfully" }
                };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        public async Task<AuthResponse> Login(LoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "User does not exist" }
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "Invalid Password" }
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("HireDate", user.HireDate.ToString()),
                new Claim("VacationDaysAccruedPerMonth", user.VacationDaysAccruedPerMonth.ToString()),
                new Claim("SickHoursAccruedPerMonth", user.SickHoursAccruedPerMonth.ToString()),
                new Claim("PersonalDaysPerYear", user.PersonalDaysPerYear.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var symmetricKey = _symmetricKeyService.GetSymmetricKey();

            string issuer;
            string audience;

            if (_env.IsDevelopment())
            {
                issuer = "https://localhost:7166/";
                audience = "https://localhost:7166/";
            }
            else
            {
                issuer = "https://timekeepapp.azurewebsites.net/";
                audience = "https://timekeepapp.azurewebsites.net/";
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                IsSuccess = true,
                Token = tokenString
            };
        }

        public async Task<List<TimeKeepUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<TimeKeepUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<TimeKeepUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AuthResponse> UpdateUser(TimeKeepUser user)
        {
            var thisUser = await GetUserById(user.Id);

            thisUser.FirstName = user.FirstName;
            thisUser.LastName = user.LastName;
            thisUser.HireDate = user.HireDate.ToUniversalTime();
            thisUser.VacationDaysAccruedPerMonth = user.VacationDaysAccruedPerMonth;
            thisUser.SickHoursAccruedPerMonth = user.SickHoursAccruedPerMonth;
            thisUser.PersonalDaysPerYear = user.PersonalDaysPerYear;

            var result = await _userManager.UpdateAsync(thisUser);

            if (result.Succeeded)
            {
                return new AuthResponse { IsSuccess = true };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "There was an unexpected error updating the profile. Please try again." }
                };
            }
        }
    }
}
