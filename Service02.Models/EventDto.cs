namespace Service02.Models
{
    public class EventDto
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime Connection { get; set; }
    }
}
