using System;
using System.Collections.Generic;
using System.Text;
using Booking.Common.Configurations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(BookingHotels.Functions.Startup))]


namespace BookingHotels.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder().Build();
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IBookingConfiguration, BookingConfiguration>();

            var bookingConfig = new BookingConfiguration(config);

            var myString = bookingConfig.ServiceBusConnectionString;
            builder.Services.PostConfigure < ServiceBusAttribute>(serviceBusOptions =>
            {
                serviceBusOptions.Connection = myString;
            });

        }
    }
}
