using Dapper;
using Microsoft.Extensions.Options;
using Service02.Features.Command.CreateEvent;
using Service02.Models;
using Xunit;

namespace Service02.Tests
{
    public class CreateEventHandlerTests: IDisposable
    {
        private IOptions<ConnectOption> _options;
        private CleanupTestFixture _cleanupTestFixture;

        public CreateEventHandlerTests()
        {
            _options = Options.Create(new ConnectOption() {
                ConnectionString = "Host=192.168.56.1;Port=5432;Database=postgres_test;Username=postgres;Password=1;",
            });

            _cleanupTestFixture = new CleanupTestFixture(_options);
        }

        [Fact]
        public async Task ValidCreateEventCommandCreateEventHandler()
        {
            // Arrange
            var commands = _cleanupTestFixture
                .GetCommandPack01()
                .ToArray();

            CreateEventHandler handler = new CreateEventHandler(_options);
            var cts = new CancellationTokenSource();

            // Act
            await handler.Handle(commands[0], cts.Token);
            await handler.Handle(commands[1], cts.Token);
            await handler.Handle(commands[2], cts.Token);

            var conn = _cleanupTestFixture.GetConnection();
            var result = conn
                .Query<EventDto>(@"SELECT 
                                    user_id AS UserId, 
                                    host(ip_address) AS IpAddress, 
                                    connection 
                                 FROM 
                                    event 
                                 WHERE 
                                    user_id=@UserId 
                                    and host(ip_address)=@IpAddress;
                                 ", commands[2])
                .ToList();
            var resultCount = result.Count;
            var @event = result.FirstOrDefault();

            // Assert
            Assert.Equal(1, resultCount);
            Assert.Equal(commands[2].Connection, @event?.Connection);
        }

        public void Dispose()
        {
            _cleanupTestFixture.Dispose();
        }
    }
}
