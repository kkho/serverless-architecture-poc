﻿using Booking.Common.Configurations;
using Booking.Common.Services;
using Booking.Common.Utilities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BookingCombination.Startup))]
namespace BookingCombination
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton(config);
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IBookingConfiguration, BookingConfiguration>();
            builder.Services.AddSingleton<SendGridService>();
            builder.Services.AddSingleton<BookFlightService>();
            builder.Services.AddSingleton<BookHotelService>();
        }
    }
}