using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using Service02.Models.Models;
using Service02.Services.IpService;
using Xunit;

namespace Service02.Tests
{
    public class IpServiceEFMoqTests
    {
        [Fact]
        public async Task ValidGetIpAddressesAsync()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event(){ UserId = 112233, IpAddress = "127.0.0.2", Connection = new DateTime(2030, 1, 1, 0, 0, 10, kind: DateTimeKind.Utc) },
                new Event(){ UserId = 112234, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 20, kind: DateTimeKind.Utc),},
                new Event(){ UserId = 112233, IpAddress = "127.0.0.1", Connection = new DateTime(2030, 1, 1, 0, 0, 30, kind: DateTimeKind.Utc),},
            };
            var validList = new string[2]{
                "127.0.0.1",
                "127.0.0.2",
            };

            var mockContext = Create.MockedDbContextFor<PostgresContext>();
            mockContext.Events.AddRange(events);
            await mockContext.SaveChangesAsync(); 

            var service = new IpServiceEF(mockContext);

            // Act
            var result = (await service.GetIpAddressesAsync(112233)).ToArray();

            var resultCount = result.Count();
            var resultCompare = !(result.Except(validList).Any()) && !(validList.Except(result).Any());

            // Assert
            Assert.Equal(validList.Length, resultCount);
            Assert.True(resultCompare, "List is valid");
        }
    }
}
