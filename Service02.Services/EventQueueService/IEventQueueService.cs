using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service02.Services.EventQueueService
{
    public interface IEventQueueService
    {
        Task CreateEventAsync(long userId, string ipAddress, DateTime connection);
    }
}
