namespace LiJunSpace.Common.Dtos.Account
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string CreateTime { get; set; }
        public string? Avatar { get; set; }
        public string? Signature { get; set; }
        public bool Sex { get; set; }
        public string Birthday { get; set; }
        public string? Email { get; set; }
        public bool OpenEmailNotice { get; set; }
        public bool TodayIsCheckIn { get; set; }
        public int ContinueCheckInDays { get; set; }
        public bool TodayIsLike { get; set; }
        public List<IntegralDto> Integrals { get; set; }
    }

    public class IntegralDto 
    {
        public string Id { get; set; }
        public IntegralType Type { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public enum IntegralType
    {
        PublishRecord = 12,
        Checkin = 5,
        Comment = 2,
        Like = 10
    }

}
