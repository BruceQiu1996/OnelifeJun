using System.ComponentModel.DataAnnotations;

namespace LiJunSpace.API.Database.Entities
{
    public interface BaseEntity<Tkey>
    {
        [Key]
        [Required]
        public Tkey Id { get; set; }
    }
}
