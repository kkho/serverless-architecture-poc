using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Common.Models
{
    public class BookHotelMessage
    {
        public string Email { get; set; }
        public string DestinationCity { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
        public int GuestQty { get; set; }
        public int RoomQty { get; set; }
    }
}
