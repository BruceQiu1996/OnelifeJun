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
    }
}
