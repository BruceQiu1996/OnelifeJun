using LiJunSpace.API.Dtos;
using LiJunSpace.API.Services;
using LiJunSpace.Common.Dtos.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiJunSpace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(AccountService accountService,ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> LoginAsync(UserLoginDto userLoginDto) 
        {
            try
            {
                var result = await _accountService.LoginAsync(userLoginDto);

                return result.ToActionResult();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Route("profile")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ProfileAsync()
        {
            try
            {
                var result = await _accountService.ProfileAsync(HttpContext.User.Identity!.Name!);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }
    }
}
