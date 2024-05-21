using LiJunSpace.API.Database.Entities;
using LiJunSpace.Common.Dtos.Account;
using LiJunSpace.Common.Dtos.Record;
using System.Text.Json;

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

        public static Record ToRecordEntity(this RecordCreationDto recordCreationDto, string publisher)
        {
            return new Record()
            {
                Publisher = publisher,
                PublishTime = DateTime.Now,
                Title = recordCreationDto.Title,
                Content = recordCreationDto.Content,
                Images = recordCreationDto.Images,
            };
        }

        public static RecordDto ToDto(this Record record, bool brief = false)
        {
            var dto = new RecordDto()
            {
                Id = record.Id,
                Title = record.Title,
                Content = string.IsNullOrEmpty(record.Content) ? null : (brief ? (record.Content.Length > 50 ? record.Content.Substring(0,50) : record.Content) : record.Content),
                PublisherId = record.Account.Id,
                PublisherDisplayName = record.Account.DisplayName,
                PublishTime = record.PublishTime,
                //Images = string.IsNullOrEmpty(record.Images) ? null : JsonSerializer.Deserialize<List<string>>(record.Images),
            };

            return dto;
        }
    }
}
