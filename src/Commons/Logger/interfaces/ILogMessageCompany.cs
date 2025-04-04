using BookPro.Domain.Entitys.Appointment.log;

namespace BookPro.Common.Logger.interfaces;

public interface ILogMessageCompany
{
    string GetMessage(LoggerCompany booking);
}
