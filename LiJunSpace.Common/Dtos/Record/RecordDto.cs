namespace LiJunSpace.Common.Dtos.Record
{
    public class RecordQueryResultDto 
    {
        public int AllCount { get; set; }
        public List<RecordDto> Records { get; set; }
    }
    public class RecordDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string PublisherId { get; set; }
        public string PublisherDisplayName { get; set; }
        public string publisherAvatar { get; set; }
        public DateTime PublishTime { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
