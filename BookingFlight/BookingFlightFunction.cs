using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Booking.Common.Constants;
using Booking.Common.Models;
using Booking.Common.Services;
using Booking.Common.Utilities;
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
        private readonly BookFlightService _bookFlightService;
        private readonly SendGridService _sendGridService;
        private readonly IBookingConfiguration _configuration;

        public BookingFlightFunction(BookFlightService bookFlightService, SendGridService sendGridService, IBookingConfiguration configuration)
        {
            _bookFlightService = bookFlightService;
            _sendGridService = sendGridService;
            _configuration = configuration;

        }

        [FunctionName("BookingFlight")]
        public async Task Run([ServiceBusTrigger("booking", "booking-flight", Connection = "ServicebusConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var bookFlightModel = JsonConvert.DeserializeObject<BookFlightMessage>(mySbMsg);
            var bookFlightResult = await _bookFlightService.BookFlights(bookFlightModel);

            await _sendGridService.SendEmailWithInformation(bookFlightModel.Email, bookFlightResult, null);
        }

        //[FunctionName("GetPosts")]
        //public async Task<IActionResult> Get(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    var bookFlightModel = new BookFlightMessage
        //    {
        //        Email = "test@test.com",
        //        CurrentCity = "OSL-sky",
        //        DestinationCity = "TYOA-sky",
        //        OutboundDate = "2019-12-01",
        //        ReturnDate = "2019-12-30"
        //    };

        //    var bookFlightResult = await _bookFlightService.BookFlights(bookFlightModel);
        //    await _sendGridService.SendEmailWithInformation(bookFlightModel.Email, bookFlightResult, null);

        //    return new OkResult();
        //}
    }
}
