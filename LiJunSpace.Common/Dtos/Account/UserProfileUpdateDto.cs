namespace LiJunSpace.Common.Dtos.Account
{
    public class UserProfileUpdateDto
    {
        public string DisplayName { get; set; }
        public string? Signature { get; set; }
        public bool Sex { get; set; }
        public DateTime Birthday { get; set; }
    }
}
