﻿using LiJunSpace.API.Database;
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

                return new ServiceResult<UserLoginResponseDto>(new UserLoginResponseDto { AccessToken = token, Id = user.Id });
            }
        }

        /// <summary>
        /// 账号信息
        /// </summary>
        /// <param name="userLoginDto">用户id</param>
        /// <returns></returns>
        public async Task<ServiceResult<UserProfileDto>> ProfileAsync(string userId)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new ServiceResult<UserProfileDto>(HttpStatusCode.BadRequest, "用户异常");

            if (string.IsNullOrEmpty(user.Avatar))
            {
                user.Avatar = user.Sex ? "default1.jpg" : "default0.jpg";
            }

            var dto = user.ToUserProfileDto();
            dto.TodayIsCheckIn = (await _junRecordDbContext.CheckInRecords.FirstOrDefaultAsync(x => x.Checker == userId &&
                x.CheckInTime.Year == DateTime.Now.Year &&
                x.CheckInTime.Month == DateTime.Now.Month &&
                x.CheckInTime.Day == DateTime.Now.Day)) != null;

            var con1 = await _junRecordDbContext.Database
                .SqlQueryRaw<int>($"SELECT COUNT(1) FROM(SELECT datediff(t.date,date_sub(curdate(),interval t.rownum day)) as dayinterval FROM (SELECT date_format(CheckInTime,'%Y-%m-%d') as date,row_number() over(ORDER BY CheckInTime DESC) as rownum FROM checkinrecords where checker = '{userId}')t)t1 WHERE t1.dayinterval=0;").ToListAsync();
            var con2 = await _junRecordDbContext.Database
                .SqlQueryRaw<int>($"SELECT COUNT(1) FROM(SELECT datediff(t.date,date_sub(curdate(),interval t.rownum day)) as dayinterval FROM (SELECT date_format(CheckInTime,'%Y-%m-%d') as date,row_number() over(ORDER BY CheckInTime DESC) as rownum FROM checkinrecords where checker = '{userId}')t)t1 WHERE t1.dayinterval=1;").ToListAsync();
            dto.ContinueCheckInDays = Math.Max(con1.FirstOrDefault(), con2.FirstOrDefault());

            return new ServiceResult<UserProfileDto>(dto);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userProfileUpdateDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateProfileAsync(UserProfileUpdateDto userProfileUpdateDto, string userId)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return new ServiceResult(HttpStatusCode.BadRequest, "用户异常");

            if (string.IsNullOrEmpty(userProfileUpdateDto.DisplayName))
            {
                return new ServiceResult(HttpStatusCode.BadRequest, "数据异常");
            }

            user.DisplayName = userProfileUpdateDto.DisplayName;
            user.Signature = userProfileUpdateDto.Signature;
            user.Sex = userProfileUpdateDto.Sex;
            user.Email = userProfileUpdateDto.Email;
            user.OpenEmailNotice = userProfileUpdateDto.OpenEmailNotice;
            user.Birthday = DateOnly.FromDateTime(userProfileUpdateDto.Birthday);

            _junRecordDbContext.Update(user);

            await _junRecordDbContext.SaveChangesAsync();

            return new ServiceResult();
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

        public async Task<ServiceResult<string>> UploadAvatarAsync(string userId, IFormFile file)
        {
            var user = await _junRecordDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return new ServiceResult<string>(HttpStatusCode.BadRequest, "用户异常");
            }

            if (file.Length > 1024 * 1024 * 10)
            {
                return new ServiceResult<string>(HttpStatusCode.BadRequest, "图片大小不符要求");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var saveFolder = _configuration.GetSection("FileStorage:AvatarImagesLocation").Value!;
            var fileName = $"{Path.GetRandomFileName()}{fileExtension}";
            var fullName = Path.Combine(saveFolder, fileName);

            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            using (var fs = File.Create(fullName))
            {
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
            }
            user.Avatar = fileName;
            _junRecordDbContext.Accounts.Update(user);
            await _junRecordDbContext.SaveChangesAsync();

            return new ServiceResult<string>(fileName);
        }
    }
}
