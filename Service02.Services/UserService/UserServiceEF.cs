using Microsoft.EntityFrameworkCore;
using Service02.Models.Models;

namespace Service02.Services.UserService
{
    public class UserServiceEF : IUserService
    {
        private PostgresContext _ctx;

        public UserServiceEF(PostgresContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<long>> GetUsersByIpAddressAsync(string ipAddress)
        {
            var result = await _ctx.Events
                .Where(e => e.IpAddress.StartsWith(ipAddress))
                .Select(e => e.UserId)
                .Distinct()
                .ToListAsync();

            return result;
        }
    }
}
