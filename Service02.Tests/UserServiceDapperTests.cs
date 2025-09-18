using Microsoft.Extensions.Options;
using Service02.Models;
using Service02.Services.UserService;
using Xunit;

namespace Service02.Tests
{
    public class UserServiceDapperTests : IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public UserServiceDapperTests()
        {
            _options = Options.Create(new ConnectOption()
            {
                ConnectionString = "Host=192.168.56.1;Port=5432;Database=postgres_test;Username=postgres;Password=1;",
            });

            _cleanupTestFixture = new CleanupTestFixture(_options);
        }

        [Fact]
        public async Task ValidGetUsersByIpAddressAsync()
        {
            // Arrange
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            // Act
            var service = new UserServiceDapper(_options);

            var result = (await service.GetUsersByIpAddressAsync(commands[0].IpAddress)).ToArray();

            var resultCount = result.Count();
            var validList = new long[2]{
                commands[1].UserId,
                commands[2].UserId,
            };
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
