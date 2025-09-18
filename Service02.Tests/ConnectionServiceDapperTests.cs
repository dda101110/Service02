using Microsoft.Extensions.Options;
using Service02.Models;
using Service02.Services.ConnectionService;
using Xunit;

namespace Service02.Tests
{
    public class ConnectionServiceDapperTests : IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public ConnectionServiceDapperTests()
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
            // Arrange
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            // Act
            var service = new ConnectionServiceDapper(_options);

            var result = await service.GetLastConnectionAsync(commands[0].UserId);

            var validEvent = commands[2];

            // Assert
            Assert.Equal(validEvent.Connection, result.Connection);
            Assert.Equal(validEvent.IpAddress, result.IpAddress);
        }

        [Fact]
        public async Task ValidGetLastTimeConnectionAsync()
        {
            // Arrange
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            var service = new ConnectionServiceDapper(_options);

            var result = await service.GetLastTimeConnectionAsync(commands[0].UserId, commands[0].IpAddress);

            var validValue = commands[2].Connection;

            // Assert
            Assert.Equal(validValue, result);
        }

        public void Dispose()
        {
            _cleanupTestFixture.Dispose();
        }
    }
}
