namespace LiJunSpace.Common.Dtos.Event
{
    public class EventCreationDto
    {
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public bool Desc { get; set; } 
        public bool UseSeconds { get; set; } 
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool ShowOnMainpage { get; set; }
    }
}
