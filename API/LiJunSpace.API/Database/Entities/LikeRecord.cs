using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiJunSpace.API.Database.Entities
{
    public class LikeRecord : BaseEntity<string>
    {
        [MaxLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Activer { get; set; }
        [ForeignKey("Activer")]
        public virtual Account ActiveAccount { get; set; }
        public string Passiver { get; set; }
        [ForeignKey("Passiver")]
        public virtual Account PassiveAccount { get; set; }
        public DateTime LikeTime { get; set; }
    }
}
