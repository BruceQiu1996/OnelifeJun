namespace LiJunSpace.Common.Dtos.Event
{
    public class EventDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public bool Desc { get; set; }
        public bool UseSeconds { get; set; }
        public DateTime CreateTime { get; set; }
        public string Publisher { get; set; }
        public string PublisherDisplayName { get; set; }
        public string PublisherAvatar { get; set; }
    }
}
