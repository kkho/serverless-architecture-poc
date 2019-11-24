using System;
using System.Net.Http;
using Booking.Common.Configurations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookingHotel
{
    public class BookingHotelFunction
    {
        private readonly HttpClient _client;
        private readonly IBookingConfiguration _configuration;

        public BookingHotelFunction(IHttpClientFactory httpClientFactory, IBookingConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _configuration = configuration;

        }

        [FunctionName("BookingHotel")]
        public static void Run([ServiceBusTrigger("booking", "booking-hotel", Connection = "")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
