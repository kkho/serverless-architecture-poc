using System.Net.Http;
using System.Threading.Tasks;
using Booking.Common.Configurations;
using Booking.Common.Models;
using Newtonsoft.Json;

namespace Booking.Common.Services
{
    public class BookHotelService
    {
        private readonly HttpClient _client;
        private readonly IBookingConfiguration _configuration;

        public BookHotelService(IHttpClientFactory httpClientFactory, IBookingConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<string> BookHotels(BookHotelMessage message)
        {
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _configuration.RapidApiKey);
            var currency = Constants.Constants.Currency;

            var getPlaces = await _client.GetAsync(
                $"https://apidojo-booking-v1.p.rapidapi.com/locations/auto-complete?text={message.DestinationCity}");

            var placeResult = await getPlaces.Content.ReadAsStringAsync();
            var placesData = JsonConvert.DeserializeObject<HotelPlaceResult[]>(placeResult);

            var getHotels = await _client.GetAsync(
                $"https://apidojo-booking-v1.p.rapidapi.com/properties/list?search_type=city&offset=0&dest_ids={placesData[0].DestId}&" +
                $"guest_qty={message.GuestQty}&arrival_date={message.ArrivalDate}&departure_date={message.DepartureDate}" +
                $"&room_qty={message.RoomQty}" +
                $"&price_filter_currencycode={currency}&travel_purpose={message.TravelPurpose}");

            return await getHotels.Content.ReadAsStringAsync();
        }
    }
}
