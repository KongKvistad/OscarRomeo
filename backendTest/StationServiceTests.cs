using backend.Models;
using backend.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Net;
using static backend.Models.StationInformation;
namespace backendTest
{


    public class StationServiceTests
    {
        [Fact]
        public async Task GetMergedStationDataAsync_ReturnsMergedStation()
        {
            // Arrange
            var fakeHttpClient = A.Fake<HttpClient>();
            var fakeLogger = A.Fake<ILogger<StationService>>();

            var fakeStationService = A.Fake<StationService>(x => x.WithArgumentsForConstructor(() => new StationService(fakeHttpClient, fakeLogger)));

            var stationInfo = new StationInformation
            {
                data = new StationInformationData
                {
                    stations = new List<StationInfo>
                {
                     new StationInfo
                    {
                        station_id = "1",
                        name = "Station 1",
                        lat = 59.911491,
                        lon = 10.757933,
                        capacity = 20
                    }
                }
                }
            };

            var stationStatus = new StationStatus
            {
                data = new StationStatus.StationStatusDataContainer
                {
                    stations = new List<StationStatus.StationStatusData>
                {
                    new StationStatus.StationStatusData
                    {
                        station_id = "1",
                        num_bikes_available = 5,
                        num_docks_available = 15
                    }
                }
                }
            };

            A.CallTo(() => fakeStationService.FetchDataFromApiAsync<StationInformation>(A<string>.That.Contains("station_information.json")))
                .Returns(Task.FromResult(stationInfo));

            A.CallTo(() => fakeStationService.FetchDataFromApiAsync<StationStatus>(A<string>.That.Contains("station_status.json")))
                .Returns(Task.FromResult(stationStatus));

            // Act
            var stations = await fakeStationService.GetMergedStationDataAsync();

            // Assert
            Assert.Single(stations);
            Assert.Equal("1", stations[0].StationId);
            Assert.Equal("Station 1", stations[0].Name);
            Assert.Equal(5, stations[0].NumBikesAvailable);
            Assert.Equal(15, stations[0].NumDocksAvailable);


        }
        [Fact]
        public async Task InvokingHttpClient_AddsClientIdentifierHeader()
        {
            // Arrange

            // Arrange
            var fakeLogger = A.Fake<ILogger<StationService>>();

            var customHandler = new CustomHeaderHandler
            {
                InnerHandler = new HttpClientHandler()
            };

            var httpClient = new HttpClient(customHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var service = new StationService(httpClient, fakeLogger);

            // Intercept the actual HTTP request to ensure the header is added
            var fakeHttpMessageHandler = A.Fake<HttpMessageHandler>();
            A.CallTo(fakeHttpMessageHandler)
                .Where(call => call.Method.Name == "SendAsync")
                .WithReturnType<Task<HttpResponseMessage>>()
                .Invokes((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    // Assert that the header is present
                    Assert.True(request.Headers.Contains("Client-Identifier"));
                    Assert.Equal("oppgave-oscarRomeo", request.Headers.GetValues("Client-Identifier").First());
                });

            var httpClientWithFakeHandler = new HttpClient(fakeHttpMessageHandler)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var serviceWithFakeHandler = new StationService(httpClientWithFakeHandler, fakeLogger);

            // Act
            var stations = await serviceWithFakeHandler.GetMergedStationDataAsync();

        }
    }

}