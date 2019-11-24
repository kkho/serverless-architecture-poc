using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Azure.ServiceBus;

namespace BookingFlights.Functions
{
    public static class BookFlightFunction
    {
        [FunctionName("BookFlight")]
        public static void Run([ServiceBusTrigger("booking", "booking-flight", Connection = "")]Message mySbMsg, 
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            Console.WriteLine("Hello World");

            // Read from api and then post answer directly as an email formatted text using sendGrid

        }
    }
}
