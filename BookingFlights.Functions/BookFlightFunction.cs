using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace BookingFlights.Functions
{
    public static class BookFlightFunction
    {
        [FunctionName("BookFlight")]
        public static void Run([ServiceBusTrigger("booking", "booking-flight", AccessRights.Manage, Connection = "")]string mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            // Read from api and then post answer directly as an email formatted text using sendGrid

        }
    }
}
