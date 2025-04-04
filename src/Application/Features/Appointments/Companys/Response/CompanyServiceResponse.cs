namespace BookPro.Application.Features.Appointments.Companys.Response;

public class CompanyServiceResponse<T>
{
    public string Message { get; set; }
    public List<T> List { get; set; }

    public CompanyServiceResponse(string message, List<T> list)
    {
        Message = message;
        List = list;
    }
}
