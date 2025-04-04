namespace BookPro.Domain.Entitys.Appointment;

public class Motel
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public virtual Company Companie { get; set; }
}
