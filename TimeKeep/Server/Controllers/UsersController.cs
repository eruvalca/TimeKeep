using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeKeep.Server.Services;
using TimeKeep.Shared;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Models;
using TimeKeep.Shared.ViewModels;

namespace TimeKeep.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly PTOEntriesService _ptoEntriesService;

        public UsersController(UsersService usersService, PTOEntriesService ptoEntriesService)
        {
            _usersService = usersService;
            _ptoEntriesService = ptoEntriesService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterVM registerModel)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _usersService.Register(registerModel);

                if (registerResult.IsSuccess)
                {
                    //try to log user in
                    var loginModel = new LoginVM
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password
                    };

                    var loginResult = await _usersService.Login(loginModel);

                    if (loginResult.IsSuccess)
                    {
                        //return token for login
                        return Ok(loginResult);
                    }
                    else
                    {
                        //if login did not work just return register result
                        return Ok(registerResult);
                    }
                }
                else
                {
                    return BadRequest(registerResult);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                            .Where(m => m.Value.Errors.Any())
                            .Select(m => m.Value.Errors.ToString())
                            .ToList()
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersService.Login(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                            .Where(m => m.Value.Errors.Any())
                            .Select(m => m.Value.Errors.ToString())
                            .ToList()
                });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<AuthResponse>> UpdateUser(UserVM user)
        {
            if (ModelState.IsValid)
            {
                var timeKeepUser = new TimeKeepUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    HireDate = user.HireDate,
                    VacationDaysAccruedPerMonth = user.VacationDaysAccruedPerMonth,
                    SickHoursAccruedPerMonth = user.SickHoursAccruedPerMonth,
                    PersonalDaysPerYear = user.PersonalDaysPerYear
                };

                var response = await _usersService.UpdateUser(timeKeepUser);

                if (response.IsSuccess)
                {
                    var ptoEntry = await _ptoEntriesService.GetVacationCarriedOverEntryByUserId(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                    if (ptoEntry is not null)
                    {
                        ptoEntry.PTOHours = (user.VacationDaysCarriedOver * 8) * -1;
                        ptoEntry = await _ptoEntriesService.UpdatePTOEntry(ptoEntry);
                    }
                    else
                    {
                        ptoEntry = new PTOEntry
                        {
                            PTOHours = (user.VacationDaysCarriedOver * 8) * -1,
                            PTOType = PTOType.VacationCarriedOver,
                            PTODate = new DateTime(DateTime.Today.Year, 1, 1).ToUniversalTime(),
                            CreateDate = DateTime.UtcNow,
                            TimeKeepUserId = user.Id
                        };

                        ptoEntry = await _ptoEntriesService.CreatePTOEntry(ptoEntry);
                    }

                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                        .Where(m => m.Value.Errors.Any())
                        .Select(m => m.Value.Errors.ToString())
                        .ToList()
                });
            }
        }
    }
}
