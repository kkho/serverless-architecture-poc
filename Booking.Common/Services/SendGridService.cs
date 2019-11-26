using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Booking.Common.Models;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Booking.Common.Utilities
{
    public class SendGridService
    {
        private IBookingConfiguration _configuration;
        public SendGridService(IBookingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailWithInformation(string email, BookFlightResult flightContent = null, string hotelContent = null)
        {
            var apiKey = _configuration.SendGridApiKey;
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bookpoc.com", "Booking information from your request"),
                Subject = "Information about your booking request",
                HtmlContent = "Sending you information in data readable format :)"
            };

            msg.AddTo(email);

            if (flightContent != null)
            {
                var json = JsonConvert.SerializeObject(flightContent);
                msg.AddAttachment(CreateAttachmentFromContent(json, "flight.json"));
            }

            if (hotelContent != null)
            {
                var json = JsonConvert.SerializeObject(hotelContent);
                msg.AddAttachment(CreateAttachmentFromContent(json, "hotel.json"));
            }

            var response = await client.SendEmailAsync(msg);
            var responseBody = await response.Body.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return true;
            }

            return false;
        }

        private Attachment CreateAttachmentFromContent(string dataContent, string filename)
        {
            return new Attachment
            {
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(dataContent)),
                Filename = filename,
                Type = "application/json",

            };
        }
    }
}
