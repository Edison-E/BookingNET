using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.interfaces.Appointment;

public interface ICompaniesRepository
{
    Task<Company> GetCompaniesByNameAndAddress(string name, string address);
    Task<List<Company>> GetAllCompanies();
    Task<bool> AddCompany(Company companieRequest, Address addressRequest, string typeService);
}
