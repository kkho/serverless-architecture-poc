using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace BookingFlights.Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("flights", "myflights", AccessRights.Manage, Connection = "")]string mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            // Read from api and then post answer directly as an email formatted text using sendGrid

        }
    }
}
