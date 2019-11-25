namespace Booking.Common.Configurations
{
    public interface IBookingConfiguration
    {
        string ServiceBusConnectionString { get; }
        string RapidApiKey { get; }
        string SkyScannerHostApi { get; }
        string SendGridApiKey { get; }
    }
}
