namespace Service02.Models
{
    public class EventDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime Connection { get; set; }
    }
}
