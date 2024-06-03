using backend.Models;
using backend.Services;
using backendTest.MockServices;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task HttpClient_Should_Have_Custom_Header()
        {
            // Arrange
            var mockHandler = new MockHttpMessageHandler();
            var fakeLogger = A.Fake<ILogger<StationService>>();
            var customHeaderHandler = new CustomHeaderHandler()
            {
                InnerHandler = mockHandler
            };

            var httpClient = new HttpClient(customHeaderHandler);

            // You would normally inject other dependencies of StationService here
            var stationService = new StationService(httpClient, fakeLogger);

            // Act
            // Call the method on StationService that makes the HTTP request
            await stationService.FetchDataFromApiAsync<StationInformation>("https://gbfs.urbansharing.com/oslobysykkel.no/station_information.json");

            // Assert
            Assert.NotNull(mockHandler.RequestMessage);
            Assert.True(mockHandler.RequestMessage.Headers.Contains("Client-Identifier"));
            var headerValues = mockHandler.RequestMessage.Headers.GetValues("Client-Identifier");
            Assert.Contains("oppgave-oscarRomeo", headerValues);
        }
    }

}