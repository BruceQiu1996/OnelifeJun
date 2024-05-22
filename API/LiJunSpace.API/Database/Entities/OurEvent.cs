using LiJunSpace.API.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiJunSpace.API.Database.Entiies
{
    public class OurEvent : BaseEntity<string>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [MaxLength(50)]
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public bool Desc { get; set; } //倒计时还是正计时
        public bool UseSeconds { get; set; } //是否使用秒的倒计时
        public DateTime CreateTime { get; set; }
        public string Publisher { get; set; }
        [ForeignKey("Publisher")]
        public virtual Account Account { get; set; }
        public bool ShowOnMainpage { get; set; } //主页展示
    }
}
