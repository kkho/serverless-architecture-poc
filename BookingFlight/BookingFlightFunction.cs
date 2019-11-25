using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Booking.Common.Constants;
using Booking.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookingFlight
{
    public class BookingFlightFunction
    {
        private readonly HttpClient _client;
        private readonly IBookingConfiguration _configuration;

        public BookingFlightFunction(IHttpClientFactory httpClientFactory, IBookingConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _configuration = configuration;

        }

        [FunctionName("BookingFlight")]
        public static void Run([ServiceBusTrigger("booking", "booking-flight", Connection = "ServicebusConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }

        [FunctionName("GetPosts")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _configuration.RapidApiKey);
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", _configuration.SkyScannerHostApi);
            var country = Constants.Country;
            var currency = Constants.Currency;
            var locale = Constants.Locale;

            var bookFlightModel = new BookFlightMessage
            {
                CurrentCity = "OSL-sky",
                DestinationCity = "TYOA-sky",
                OutboundDate = "2019-12-01",
                ReturnDate = "2020-01-30"
            };

           // var getRelevantPlaces = await _client.GetAsync(
           //     $"https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/autosuggest/v1.0/{country}/{currency}/{locale}/?query={bookFlightModel.City}");

           //var content = await getRelevantPlaces.Content.ReadAsStringAsync();
           //var places = JsonConvert.DeserializeObject<PlaceCollection>(content);

           var getRoutes = await _client.GetAsync(
           $" https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/browseroutes/v1.0/" +
           $"{country}/{currency}/{locale}/{bookFlightModel.CurrentCity}/{bookFlightModel.DestinationCity}/{bookFlightModel.OutboundDate}/{bookFlightModel.ReturnDate}");

            var content = await getRoutes.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BookFlightResult>(content);

            return new OkResult();
        }
    }
}
