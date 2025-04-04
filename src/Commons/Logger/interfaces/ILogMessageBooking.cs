using BookPro.Domain.Entitys.Appointment.log;

namespace BookPro.Common.Logger.interfaces;

public interface ILogMessageBooking
{
    string GetMessage(LoggerBooking booking);
}
