using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiJunSpace.API.Database.Entities
{
    /// <summary>
    /// 日常记录
    /// </summary>
    public class Record : BaseEntity<string>
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [MaxLength(16)]
        public string? Title { get; set; }
        [MaxLength(4000)]
        public string? Content { get; set; }
        public string Publisher { get; set; }
        [ForeignKey("Publisher")]
        public virtual Account Account { get; set; }
        public DateTime PublishTime { get; set; }
        public string? Images { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Comment> Comments { get; set;}
    }
}
