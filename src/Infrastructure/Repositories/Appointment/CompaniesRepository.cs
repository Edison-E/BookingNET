using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Repositories.Appointment;

public class CompaniesRepository : Repository<Company>, ICompaniesRepository
{
    private readonly IAddressRepository _addressRepository;
    private readonly IServicesRepository _servicesRepository;
    public CompaniesRepository(
        ApplicationDbContext context, 
        IAddressRepository addressRepository, 
        IServicesRepository servicesRepository) : base(context)
    {
        _addressRepository = addressRepository;
        _servicesRepository = servicesRepository;
    }

    public async Task<List<Company>> GetAllCompanies()
    {
        return await _context.Companies
            .Include(comp => comp.Address)
            .ToListAsync();
    }

    public async Task<Company> GetCompaniesByNameAndAddress(string name, string address)
    {
        return await _context.Companies
            .Where(x => x.Name.Equals(name) && x.Address.Street.Equals(address))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> AddCompany(Company company, Address address, string typeServiceName)
    {
        var isAddAddress = await _addressRepository.InsertAddress(address);
        if (!isAddAddress)
        {
            return false;
        }

        var addressCompany = await _addressRepository.GetAddressbyStreet(address.Street);

        company.AddressId = addressCompany.Id;

        var typeService = await _servicesRepository.GetServiceByName(typeServiceName);
        company.ServiceId = typeService.Id;


       return await base.Insert(company);
    }
}
