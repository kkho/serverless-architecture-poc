using System;
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

namespace BookingHotel
{
    public class BookingHotelFunction
    {
        private readonly BookHotelService _bookHotelService;
        private readonly SendGridService _sendGridService;
        private readonly IBookingConfiguration _configuration;

        public BookingHotelFunction(BookHotelService bookHotelService, SendGridService sendGridService, IBookingConfiguration configuration)
        {
            _bookHotelService = bookHotelService;
            _sendGridService = sendGridService;
            _configuration = configuration;

        }

        [FunctionName("BookingHotel")]
        public async Task Run([ServiceBusTrigger("booking", "booking-hotel", Connection = "ServicebusConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}"); log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var bookHotelModel = JsonConvert.DeserializeObject<BookHotelMessage>(mySbMsg);

            var hotelResult = await _bookHotelService.BookHotels(bookHotelModel);

            await _sendGridService.SendEmailWithInformation(bookHotelModel.Email, null, hotelResult);
        }

        //[FunctionName("GetPosts")]
        //public async Task<IActionResult> Get(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    var bookHotelModel = new BookHotelMessage
        //    {
        //        Email = "test@test.com",
        //        DestinationCity = "Tokyo",
        //        ArrivalDate = "2019-12-01",
        //        DepartureDate = "2019-12-30",
        //        GuestQty = 2,
        //        RoomQty = 1,
        //        TravelPurpose = "leisure"
        //    };

        //    var bookHotelResult = await _bookHotelService.BookHotels(bookHotelModel);
        //    await _sendGridService.SendEmailWithInformation(bookHotelModel.Email, null, bookHotelResult);

        //    return new OkResult();
        //}
    }
}
