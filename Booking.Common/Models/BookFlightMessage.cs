using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Common.Models
{
    public class BookFlightMessage
    {
        public string Email { get; set; }
        public string CurrentCity { get; set; }
        public string DestinationCity { get; set; }
        public string OutboundDate { get; set; }
        public string ReturnDate { get; set; }
    }
}
