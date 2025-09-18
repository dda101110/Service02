using Microsoft.EntityFrameworkCore;
using Service02.Models.Models;

namespace Service02.Services.IpService
{
    public class IpServiceEF : IIpService
    {
        private PostgresContext _ctx;

        public IpServiceEF(PostgresContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<string>> GetIpAddressesAsync(long userId)
        {
            var result = await _ctx.Events
                .Where(e => e.UserId==userId)
                .Select(e => e.IpAddress)
                .ToListAsync();

            return result;
        }
    }
}
