using System;
using System.Collections.Generic;
using System.Text;
using Booking.Common.Utilities;
using Microsoft.Extensions.Configuration;

namespace Booking.Common.Configurations
{
   public class BookingConfiguration : IBookingConfiguration
   {
       private IConfiguration _configuration;

       public BookingConfiguration(IConfiguration configuration)
       {
           _configuration = configuration;
       }

       public string GetSettingAsString(string name)
       {
           return _configuration[$"{name}"];
       }

       public string GetSecretAsString(string name)
       {
           var cacheDate = new DateTimeOffset(DateTime.Now.AddDays(15));
           return (KeyVaultUtility.GetCachedSecret(this.GetSettingAsString("KeyVaultUri"), name, "AzureBookingKeyCacheCache_" + name, cacheDate).Result);
       }

       public string ServiceBusConnectionString => (GetSecretAsString("ServiceBusConnectionString"));
    }
}
