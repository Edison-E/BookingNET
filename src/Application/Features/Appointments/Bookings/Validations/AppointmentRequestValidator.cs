using BookPro.Application.Features.Appointments.Companys.DTOs;
using System.Globalization;

namespace BookPro.Application.Features.Appointments.Bookings.Validations;

public class AppointmentRequestValidator : BaseValidation<AppointmentBaseRequest>, IAppointmentRequestValidator
{
    public ValidationResult ValidateRequest(AppointmentBaseRequest request)
    {
        ValidationResult result = new ValidationResult();

        if (request is MotelRequestDTO requestMotel)
        {
            return ValidateMotelRequest(requestMotel);
        }

        result.Message.AddRange(ValidateEntity(request, "Request").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.CustomerEmail, "Email").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.CompanyName, "Name Company").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Date, "Check in").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Hour, "Hour").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Address, "Address").Message);

        return result;
    }

    private ValidationResult ValidateMotelRequest(MotelRequestDTO request)
    {
        ValidationResult result = new ValidationResult();

        result.Message.AddRange(ValidateParameterRequiredString(request.CustomerEmail, "Email").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.CompanyName, "Name Company").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Date, "Check in").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Hour, "Hour").Message);
        result.Message.AddRange(ValidateParameterRequiredString(request.Address, "Address").Message);
        result.Message.AddRange(ValidateCheckInCheckOut(request).Message);

        return result;
        
    }
    public ValidationResult ValidateCheckInCheckOut(MotelRequestDTO request)
    {
        ValidationResult result = new ValidationResult();

        DateTime checkIn = DateTime.ParseExact(request.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        DateTime checkOut = DateTime.ParseExact(request.CheckOut, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        if (checkOut <= checkIn)
        {
            result.Message.Add("Check-out date must be after check-in date.");
        }

        return result;
    }
}
