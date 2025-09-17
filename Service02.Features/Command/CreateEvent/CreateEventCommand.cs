using MediatR;

namespace Service02.Features.Command.CreateEvent
{
    public class CreateEventCommand: IRequest
    {
        public long UserId { get; set; }
        public string IpAddress {  get; set; }
        public DateTime Connection {  get; set; }
    }
}
