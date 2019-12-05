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
           return (KeyVaultUtility.GetSecret(this.GetSettingAsString("KeyVaultUri"), name).Result);
       }

       public string ServiceBusConnectionString => (GetSecretAsString("ServicebusConnectionString"));
       public string RapidApiKey => (GetSecretAsString("rapidApiKey"));
       public string SkyScannerHostApi => (GetSettingAsString("SkyScannerHostApi"));
       public string SendGridApiKey => (GetSecretAsString("sendGridApiKey"));
   }
}
