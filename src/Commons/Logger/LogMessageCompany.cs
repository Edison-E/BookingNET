using BookPro.Common.Logger.interfaces;
using BookPro.Domain.Entitys.Appointment.log;

namespace BookPro.Common.Logger;

public class LogMessageCompany : ILogMessageCompany
{
    private readonly Dictionary<LoggerCompany, string> _messages = new()
    {
        {LoggerCompany.ErrorGetCompanies, "The companies could not be obtained."},
        {LoggerCompany.ErrorGetRooms, "Unable to obtain rooms from this company."},
        {LoggerCompany.ErrorProcessInsertAddress, "Error inserting the new address."},
        {LoggerCompany.ErrorProcessInsertCompany, "Error inserting the new company."},
        {LoggerCompany.RequestAddressInvalid, "The request address is invalid, check the parameters:"},
        {LoggerCompany.RequestCompanyInvalid, "The request company is invalid, check the parameters:"},
        {LoggerCompany.AddressInvalid, "The address model is invalid:"},
        {LoggerCompany.CompanyInvalid, "The company model is invalid:"},
        {LoggerCompany.AddressNotInsert, "The new address could not be inserted correctly."},
        {LoggerCompany.CompanyNotInsert, "The new company could not be inserted correctly."},
        {LoggerCompany.AddressExist, "This address is occupied by another company, check the address provided."}
    };

    public string GetMessage(LoggerCompany company)
    {
        return _messages[company];
    }
}
