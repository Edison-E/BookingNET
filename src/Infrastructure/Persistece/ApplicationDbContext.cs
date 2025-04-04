using BookPro.Domain.Entitys.Appointment;

namespace BookPro.Infrastructure.Persistece;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    //Authentication
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    //Appointment
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<TypeServices> TypeServices { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Motel> Motels { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(t => t.Company)
            .WithMany()
            .HasForeignKey(a => a.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.Address)
            .WithMany()
            .HasForeignKey(c => c.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.TypeService)
            .WithMany()
            .HasForeignKey(c => c.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Motel>()
           .HasOne(m => m.Companie)
           .WithOne()
           .HasForeignKey<Motel>(m => m.CompanyId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.Motel)
            .WithMany()
            .HasForeignKey(r => r.MotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
