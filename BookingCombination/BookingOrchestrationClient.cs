using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Booking.Common.Models;
using Booking.Common.Services;
using Booking.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookingCombination
{
    public class BookingOrchestrationClient
    {
        private readonly BookFlightService _bookFlightService;
        private readonly BookHotelService _bookHotelService;
        private readonly SendGridService _sendGridService;
        private readonly IBookingConfiguration _configuration;

        public BookingOrchestrationClient(
            BookFlightService bookFlightService,
            BookHotelService bookHotelService,
            SendGridService sendGridService,
            IBookingConfiguration configuration)
        {
            _bookFlightService = bookFlightService;
            _bookHotelService = bookHotelService;
            _sendGridService = sendGridService;
            _configuration = configuration;

        }



        [FunctionName("BookingOrchestrationClient")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var myMsb = context.GetInput<string>();
            var messageContent = JsonConvert.DeserializeObject<BookCombinationMessage>(myMsb);

            var flightModel = new BookFlightMessage
            {
                Email = messageContent.Email,
                CurrentCity = messageContent.CurrentCityCode,
                DestinationCity = messageContent.DestinationCode,
                OutboundDate = messageContent.OutboundDate,
                ReturnDate = messageContent.ReturnDate
            };

            var hotelModel = new BookHotelMessage
            {
                Email = messageContent.Email,
                ArrivalDate = messageContent.OutboundDate,
                DepartureDate = messageContent.ReturnDate,
                DestinationCity = messageContent.DestinationCity,
                GuestQty = messageContent.GuestQty,
                RoomQty = messageContent.RoomQty,
                TravelPurpose = messageContent.TravelPurpose
            };

            string flightContent = null;
            string hotelContent = null;

            var parallelTasks = new List<Task<KeyValuePair<string, string>>>();
            parallelTasks.Add(context.CallActivityAsync<KeyValuePair<string, string>>("BookFlightActivity", flightModel));
            parallelTasks.Add(context.CallActivityAsync<KeyValuePair<string, string>>("BookHotelActivity", hotelModel));

            await Task.WhenAll(parallelTasks);

            parallelTasks.ForEach(t =>
            {
                var result = t.Result;
                if (result.Key.Equals("flight"))
                {
                    flightContent = result.Value;
                } 
                else if (result.Key.Equals("hotel"))
                {
                    hotelContent = result.Value;
                }
            });


            await _sendGridService.SendEmailWithInformation(messageContent.Email, flightContent, hotelContent);
        }

        [FunctionName("BookFlightActivity")]
        public async Task<KeyValuePair<string, string>> BookFlight([ActivityTrigger] BookFlightMessage content, ILogger log)
        {
            log.LogInformation($"Starting BookFlight Activity");
            var result = await _bookFlightService.BookFlights(content);
            return new KeyValuePair<string, string>("flight", result);
        }


        [FunctionName("BookHotelActivity")]
        public async Task<KeyValuePair<string, string>> BookHotel([ActivityTrigger] BookHotelMessage content, ILogger log)
        {
            log.LogInformation($"Starting BookHotel Activity");
            var result = await _bookHotelService.BookHotels(content);
            return new KeyValuePair<string, string>("hotel", result);
        }

        [FunctionName("BookingOrchestrationClient_BookingCombinationStart")]
        public async Task BookingCombinationStart(
            [ServiceBusTrigger("booking", "booking-combination", Connection = "ServicebusConnectionString")]string mySbMsg,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("BookingOrchestrationClient", mySbMsg);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}