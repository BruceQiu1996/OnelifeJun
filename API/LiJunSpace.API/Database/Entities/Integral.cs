using LiJunSpace.Common.Dtos.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiJunSpace.API.Database.Entities
{
    /// <summary>
    /// 积分表
    /// </summary>
    public class Integral : BaseEntity<string>
    {
        [MaxLength(50)]
        public string Id { get; set; }  = Guid.NewGuid().ToString();
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public IntegralType Type { get; set; }
        [MaxLength(50)]
        public string Publisher { get; set; }
        [ForeignKey("Publisher")]
        public virtual Account Account { get; set; }
    }
}
