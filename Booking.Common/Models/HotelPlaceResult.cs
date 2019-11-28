using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Booking.Common.Models
{
    public class HotelPlaceResult
    {
        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty(PropertyName = "dest_id")]
        public string DestId { get; set; }
    }
}
