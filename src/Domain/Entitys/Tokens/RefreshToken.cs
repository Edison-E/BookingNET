namespace BookPro.Domain.Entitys.Tokens;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public string CreatedByIp { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;

    public int UserId { get; set; }
    public virtual User User { get; set; }
}
