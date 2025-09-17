using Microsoft.Extensions.Options;
using Service02.Models;
using Service02.Services.ConnectionService;
using Xunit;

namespace Service02.Tests
{
    public class ConnectionServiceTests : IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public ConnectionServiceTests()
        {
            _options = Options.Create(new ConnectOption()
            {
                ConnectionString = "Host=192.168.56.1;Port=5432;Database=postgres_test;Username=postgres;Password=1;",
            });

            _cleanupTestFixture = new CleanupTestFixture(_options);
        }

        [Fact]
        public async Task ValidGetLastConnectionAsync()
        {
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            var service = new ConnectionService(_options);

            var result = await service.GetLastConnectionAsync(commands[0].UserId);

            var validEvent = commands[2];

            Assert.Equal(validEvent.Connection, result.Connection);
            Assert.Equal(validEvent.IpAddress, result.IpAddress);
        }

        [Fact]
        public async Task ValidGetLastTimeConnectionAsync()
        {
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            var service = new ConnectionService(_options);

            var result = await service.GetLastTimeConnectionAsync(commands[0].UserId, commands[0].IpAddress);

            var validValue = commands[2].Connection;

            Assert.Equal(validValue, result);
        }

        public void Dispose()
        {
            _cleanupTestFixture.Dispose();
        }
    }
}
