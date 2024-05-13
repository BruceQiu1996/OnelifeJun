using LiJunSpace.API.Database;
using LiJunSpace.API.Database.Entities;
using LiJunSpace.API.Dtos;
using LiJunSpace.API.Helpers;
using LiJunSpace.Common.Dtos.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace LiJunSpace.API.Services
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public class AccountService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;
        private readonly PasswordHelper _passwordHelper;
        private readonly IConfiguration _configuration;
        public AccountService(JunRecordDbContext junRecordDbContext, PasswordHelper passwordHelper, IConfiguration configuration)
        {
            _junRecordDbContext = junRecordDbContext;
            _passwordHelper = passwordHelper;
            _configuration = configuration;
        }

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="userLoginDto">用户登录信息</param>
        /// <returns></returns>
        public async Task<ServiceResult<UserLoginResponseDto>> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x => x.UserName == userLoginDto.UserName);
            if (user == null)
                return new ServiceResult<UserLoginResponseDto>(HttpStatusCode.BadRequest, "用户名或密码错误");

            if (!_passwordHelper.VerifyHashedPassword(userLoginDto.Password, user.Password))
            {
                return new ServiceResult<UserLoginResponseDto>(HttpStatusCode.BadRequest, "用户名或密码错误");
            }
            else
            {
                var token = CreateToken(user);

                return new ServiceResult<UserLoginResponseDto>(new UserLoginResponseDto { AccessToken = token});
            }
        }

        /// <summary>
        /// 账号信息
        /// </summary>
        /// <param name="userLoginDto">用户id</param>
        /// <returns></returns>
        public async Task<ServiceResult<UserProfileDto>> ProfileAsync(string userId)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x=>x.Id== userId);
            if(user==null)
                return new ServiceResult<UserProfileDto>(HttpStatusCode.BadRequest, "用户异常");

            if (string.IsNullOrEmpty(user.Avatar)) 
            {
                user.Avatar = user.Sex ? "default1.jpg" : "default0.jpg";
            }
            return new ServiceResult<UserProfileDto>(user.ToUserProfileDto());
        }

        public string CreateToken(Account user)
        {
            var claims = new[]
             {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Sid,user.UserName),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:JwtSecret"]));
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(secretKey, algorithm);
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["System:Name"],    //Issuer
                _configuration["Jwt:Audience"],   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddSeconds(_configuration.GetValue<int>("Jwt:TokenExpireSeconds")),    //expires
                signingCredentials               //Credentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
