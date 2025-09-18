using Microsoft.EntityFrameworkCore;
using Service02.Models;
using Service02.Models.Models;
using Service02.Services.ConnectionService;
using Xunit;

namespace Service02.Tests
{
    public class ConnectionServiceEFTests
    {
        [Fact]
        public async Task ValidGetLastConnectionAsync()
        {
            // Arrange
            var events = new List<Event>()
            {
                new Event(){ UserId = 112233, IpAddress = "127.0.0.2", Connection = new DateTime(2030, 1, 1, 0, 0, 10, kind: DateTimeKind.Utc) },
                new Event(){ UserId = 112234, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 20, kind: DateTimeKind.Utc),},
                new Event(){ UserId = 112233, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 30, kind: DateTimeKind.Utc),},
            };
            var validEvent = new ConnectionResponse()
            {
                IpAddress = "127.0.0.1",
                Connection = new DateTime(2030, 1, 1, 0, 0, 30, kind: DateTimeKind.Utc),
            };

            var options = new DbContextOptionsBuilder<PostgresContext>()
                .UseInMemoryDatabase(databaseName: "TestDB1")
                .Options;

            using (var ctx = new PostgresContext(options))
            {
                ctx.Events.AddRange(events);
                await ctx.SaveChangesAsync();
            }

            using var context = new PostgresContext(options);
            var service = new ConnectionServiceEF(context);

            // Act
            var result = await service.GetLastConnectionAsync(112233);

            // Assert
            Assert.Equal(validEvent.Connection, result.Connection);
            Assert.Equal(validEvent.IpAddress, result.IpAddress);
        }

        [Fact]
        public async Task ValidGetLastTimeConnectionAsync()
        {
            // Arrange
            var events = new List<Event>()
            {
                new Event(){ UserId = 112233, IpAddress = "127.0.0.2", Connection = new DateTime(2030, 1, 1, 0, 0, 10, kind: DateTimeKind.Utc) },
                new Event(){ UserId = 112234, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 20, kind: DateTimeKind.Utc),},
                new Event(){ UserId = 112233, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 30, kind: DateTimeKind.Utc),},
            };
            var validValue = new DateTime(2030, 1, 1, 0, 0, 10, kind: DateTimeKind.Utc);

            var options = new DbContextOptionsBuilder<PostgresContext>()
                .UseInMemoryDatabase(databaseName: "TestDB2")
                .Options;

            using (var ctx = new PostgresContext(options))
            {
                ctx.Events.AddRange(events);
                await ctx.SaveChangesAsync();
            }

            using var context = new PostgresContext(options);
            var service = new ConnectionServiceEF(context);

            // Act
            var result = await service.GetLastTimeConnectionAsync(112233, "127.0.0.2");

            // Assert
            Assert.Equal(validValue, result);
        }
    }
}
