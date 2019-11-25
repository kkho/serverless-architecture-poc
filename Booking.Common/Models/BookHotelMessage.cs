using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Common.Models
{
    public class BookHotelMessage
    {
        public string Email { get; set; }
        public string DestinationCity { get; set; }
        public string CheckInDate { get; set; }
        public string CheckoutDate { get; set; }
    }
}
