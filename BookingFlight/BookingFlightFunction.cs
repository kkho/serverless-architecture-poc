using System;
using System.Net.Http;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            var res = await _client.GetAsync("https://microsoft.com");

            return new OkResult();
        }
    }
}
