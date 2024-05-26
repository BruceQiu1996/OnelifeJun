using System.ComponentModel.DataAnnotations;

namespace LiJunSpace.API.Database.Entities
{
    public class Account : BaseEntity<string>
    {
        [MaxLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(16)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(16)]
        public string DisplayName { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        
        public string? Avatar { get; set; }
        public string? Signature { get; set; }
        public bool Sex { get; set; }
        public DateOnly Birthday { get; set; }
        [MaxLength(500)]
        public string? Email { get; set; }
        public bool OpenEmailNotice { get; set; }
    }
}
