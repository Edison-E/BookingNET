using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Domain.Entitys.Users;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string DateBirth { get; set; }
    public int PhoneNumber { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime LastFailedLoginAttempt { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Appointment.Appointment> Appointments { get; set; }
}
