using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Booking.Common.Models;
using Booking.Common.Utilities;
using Newtonsoft.Json;

namespace Booking.Common.Services
{
    public class BookFlightService
    {
        private readonly HttpClient _client;
        private readonly IBookingConfiguration _configuration;

        public BookFlightService(IHttpClientFactory httpClientFactory, IBookingConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<string> BookFlights(BookFlightMessage message)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _configuration.RapidApiKey);
            var country = Constants.Constants.Country;
            var currency = Constants.Constants.Currency;
            var locale = Constants.Constants.Locale;



            var getRoutes = await _client.GetAsync(
                $" https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/browseroutes/v1.0/" +
                $"{country}/{currency}/{locale}/{message.CurrentCity}/{message.DestinationCity}/{message.OutboundDate}/{message.ReturnDate}");

            return await getRoutes.Content.ReadAsStringAsync();
        }
    }
}
