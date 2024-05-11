﻿using System.ComponentModel.DataAnnotations;
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
        public string? Image_1 { get; set; }
        public string? Image_2 { get; set; }
        public string? Image_3 { get; set; }
        public string? Image_4 { get; set; }
        public string? Image_5 { get; set; }
        public string? Image_6 { get; set; }
        public string? Image_7 { get; set; }
        public string? Image_8 { get; set; }
        public string? Image_9 { get; set; }
        public bool IsDeleted { get; set; }
    }
}
