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
        private readonly CheckInService _checkInService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(AccountService accountService, ILogger<AccountController> logger, CheckInService checkInService)
        {
            _accountService = accountService;
            _logger = logger;
            _checkInService = checkInService;
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

        [Route("profile/{id}")] 
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ProfileAsync(string id)
        {
            try
            {
                var result = await _accountService.ProfileAsync(id);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [HttpPost("edit")]
        [Authorize]
        public async Task<ActionResult> EditProfileAsync(UserProfileUpdateDto userProfileUpdateDto)
        {
            try
            {
                var result = await _accountService.UpdateProfileAsync(userProfileUpdateDto, HttpContext.User.Identity!.Name!);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Authorize]
        [HttpPost("upload-avatar")]
        public async Task<ActionResult> UploadAvatars([FromForm] IFormFile file)
        {
            try
            {
                var result = await _accountService.UploadAvatarAsync(HttpContext.User.Identity!.Name!, file);

                return result.ToActionResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem();
            }
        }

        [Authorize]
        [HttpPost("check-in")]
        public async Task<ActionResult> CheckIn()
        {
            try
            {
                var result = await _checkInService.CheckInAsync(HttpContext.User.Identity!.Name!);

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
