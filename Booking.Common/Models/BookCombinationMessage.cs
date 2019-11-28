namespace Booking.Common.Models
{
    public class BookCombinationMessage
    {
        public string Email { get; set; }
        public string CurrentCityCode { get; set; }
        public string DestinationCode { get; set; }
        public string DestinationCity { get; set; }
        public string OutboundDate { get; set; }
        public string ReturnDate { get; set; }
        public int GuestQty { get; set; }
        public int RoomQty { get; set; }
        public string TravelPurpose { get; set; }
    }
}
