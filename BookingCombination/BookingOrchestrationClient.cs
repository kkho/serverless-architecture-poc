using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BookingCombination
{
    public static class BookingOrchestrationClient
    {
        [FunctionName("BookingOrchestrationClient")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("BookingOrchestrationClient_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("BookingOrchestrationClient_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("BookingOrchestrationClient_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("BookingOrchestrationClient_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("BookingOrchestrationClient_BookingCombinationStart")]
        public static async Task BookingCombinationStart(
            [ServiceBusTrigger("booking", "booking-combination", Connection = "ServicebusConnectionString")]string mySbMsg,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("BookingOrchestrationClient", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}