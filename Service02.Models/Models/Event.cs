namespace Service02.Models.Models;

public partial class Event
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string IpAddress { get; set; }

    public DateTime Connection { get; set; }
}
