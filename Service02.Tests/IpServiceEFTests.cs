using Microsoft.EntityFrameworkCore;
using Service02.Models.Models;
using Service02.Services.IpService;
using Xunit;

namespace Service02.Tests
{
    public class IpServiceEFTests
    {
        public IpServiceEFTests()
        {
        }

        [Fact]
        public async Task ValidGetIpAddressesAsync()
        {
            // Arrange
            var events = new List<Event>()
            {
                new Event(){ UserId = 112233, IpAddress = "127.0.0.2", Connection = new DateTime(2030, 1, 1, 0, 0, 10, kind: DateTimeKind.Utc) },
                new Event(){ UserId = 112234, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 20, kind: DateTimeKind.Utc),},
                new Event(){ UserId = 112233, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 30, kind: DateTimeKind.Utc),},
            };

            var options = new DbContextOptionsBuilder<PostgresContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using (var ctx = new PostgresContext(options))
            {
                ctx.Events.AddRange(events);
                await ctx.SaveChangesAsync();
            }

            using var context = new PostgresContext(options);

            // Act
            var service = new IpServiceEF(context);

            var result = (await service.GetIpAddressesAsync(112233)).ToArray();

            var resultCount = result.Count();
            var validList = new string[2]{
                "127.0.0.1",
                "127.0.0.2",
            };
            var resultCompare = !(result.Except(validList).Any()) && !(validList.Except(result).Any());

            // Assert
            Assert.Equal(validList.Length, resultCount);
            Assert.True(resultCompare, "List is valid");
        }
    }
}
