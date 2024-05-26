using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiJunSpace.API.Database.Entities
{
    public class CheckInRecord : BaseEntity<string>
    {
        [MaxLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Checker { get; set; }
        [ForeignKey("Checker")]
        public virtual Account Account { get; set; }
        public DateTime CheckInTime { get; set; }
    }
}
