﻿namespace LiJunSpace.Common.Dtos.Account
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
    }
}
