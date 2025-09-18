using Microsoft.Extensions.Options;
using Service02.Models;
using Service02.Services.IpService;
using Xunit;

namespace Service02.Tests
{
    public class IpServiceDapperTests : IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public IpServiceDapperTests()
        {
            _options = Options.Create(new ConnectOption()
            {
                ConnectionString = "Host=192.168.56.1;Port=5432;Database=postgres_test;Username=postgres;Password=1;",
            });

            _cleanupTestFixture = new CleanupTestFixture(_options);
        }

        [Fact]
        public async Task ValidGetIpAddressesAsync()
        {
            // Arrange
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            var validList = new string[1]{
                commands[1].IpAddress,
            };

            var service = new IpServiceDapper(_options);

            // Act
            var result = (await service.GetIpAddressesAsync(commands[0].UserId)).ToArray();

            var resultCount = result.Count();
            var resultCompare = !(result.Except(validList).Any()) && !(validList.Except(result).Any());

            // Assert
            Assert.Equal(validList.Length, resultCount);
            Assert.True(resultCompare, "List is valid");
        }

        public void Dispose()
        {
            _cleanupTestFixture.Dispose();
        }
    }
}
