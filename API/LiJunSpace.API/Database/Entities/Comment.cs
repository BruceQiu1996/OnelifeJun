using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiJunSpace.API.Database.Entities
{
    public class Comment : BaseEntity<string>
    {
        [MaxLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [MaxLength(250)]
        public string Content { get; set; }
        public string Publisher { get; set; }
        [ForeignKey("Publisher")]
        public virtual Account Account { get; set; }
        public string RecordId { get; set; }
        [ForeignKey("RecordId")]
        public virtual Record Record { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
