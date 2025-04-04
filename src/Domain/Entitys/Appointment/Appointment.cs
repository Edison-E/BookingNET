namespace BookPro.Domain.Entitys.Appointment;

public class Appointment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Hour { get; set; }
    public DateTime Created {  get; set; }
    public string Status { get; set; }

    public int CustomerId { get; set; }
    public virtual User User { get; set; }
    public int CompanyId {  get; set; }
    public virtual Company Company { get; set; }
    
}
