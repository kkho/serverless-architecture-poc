using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Booking.Common.Configurations;
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

        public async Task SendEmailWithInformation(string email, string content)
        {
            var apiKey = _configuration.SendGridApiKey;
            var client = new SendGridClient(apiKey);


            var htmlContentPath = "Content/ContentPage.html";
            var htmlContent = System.IO.File.ReadAllText(htmlContentPath);

            htmlContent = htmlContent.Replace("{1}", content);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bookpoc.com", "Booking information"),
                Subject = "Information about your booking request",
                HtmlContent = htmlContent
            };

            msg.AddTo(email);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == HttpStatusCode.OK)
            {

            }


        }
    }
}
