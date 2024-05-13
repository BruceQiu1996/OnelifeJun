namespace LiJunSpace.Common.Dtos.Record
{
    public class RecordQueryDto
    {
        public int Page { get; set; }
        public string Key { get; set; }
        public bool TimeDesc { get; set; } = true;
    }
}
