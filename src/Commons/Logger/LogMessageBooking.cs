using BookPro.Common.Logger.interfaces;
using BookPro.Domain.Entitys.Appointment.log;

namespace BookPro.Common.Logger;

public class LogMessageBooking : ILogMessageBooking
{
    private readonly Dictionary<LoggerBooking, string> _messages = new()
    {
        {LoggerBooking.RequestInvalid, "The properties of the request are invalid."},
        {LoggerBooking.ErrorProcessingAppointment, "An error occurred while processing the request."},
        {LoggerBooking.ErrorUpdateRoom, "An error occurred when updating the room."},
        {LoggerBooking.ErrorSaveAppointment, "An error occurred while saving the Appointment."}
    };

    public string GetMessage(LoggerBooking booking)
    {
        return _messages[booking];
    }
}
