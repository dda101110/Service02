using Microsoft.Extensions.Options;
using Service02.Models;
using Service02.Services.IpService;
using Xunit;

namespace Service02.Tests
{
    public class IpServiceTest : IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public IpServiceTest()
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
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            await _cleanupTestFixture
                .SeedEventsAsync(commands);

            var service = new IpService(_options);

            var result = (await service.GetIpAddressesAsync(commands[0].UserId)).ToArray();

            var resultCount = result.Count();
            var validList = new string[1]{
                commands[1].IpAddress,
            };
            var resultCompare = !(result.Except(validList).Any()) && !(validList.Except(result).Any());

            Assert.Equal(validList.Length, resultCount);
            Assert.True(resultCompare, "List is valid");
        }

        public void Dispose()
        {
            _cleanupTestFixture.Dispose();
        }
    }
}
