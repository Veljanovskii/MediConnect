using Application.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<MedicalHistory> MedicalHistories { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<TreatmentRoom> TreatmentRooms { get; set; }
    public DbSet<TreatmentMachine> TreatmentMachines { get; set; }
    public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }

    public DatabaseFacade Database => base.Database;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Here you can include additional logic if needed, such as updating audit fields before saving
        return base.SaveChangesAsync(cancellationToken);
    }
}