using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace backendTest
{


    public class StationServiceTests
    {
        [Fact]
        public async Task GetStationsAsync_ReturnsStations()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockLogger = new Mock<ILogger<StationService>>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{ \"station_id\": \"1\", \"name\": \"Station 1\", \"lat\": 59.911491, \"lon\": 10.757933, \"capacity\": 20, \"num_bikes_available\": 5, \"num_docks_available\": 15 }]"),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var service = new StationService(httpClient, mockLogger.Object);

            // Act
            var stations = await service.GetMergedStationDataAsync();

            // Assert
            Assert.Single(stations);
            
        }
    }
}