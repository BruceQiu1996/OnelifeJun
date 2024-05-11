using LiJunSpace.API.Database.Entities;
using LiJunSpace.Common.Dtos.Account;

namespace LiJunSpace.API.Dtos
{
    public static class DtoEntityExtension
    {
        public static UserProfileDto ToUserProfileDto(this Account account)
        {
            return new UserProfileDto()
            {
                Id = account.Id,
                DisplayName = account.DisplayName,
                UserName = account.UserName,
                Sex = account.Sex,
                Birthday = account.Birthday.ToString("yyyy-MM-dd"),
                CreateTime = account.CreateTime.ToString("yyyy-MM-dd"),
                Signature = account.Signature,
                Avatar = account.Avatar,
            };
        }
    }
}
