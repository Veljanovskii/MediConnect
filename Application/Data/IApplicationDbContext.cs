using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Data;

public interface IApplicationDbContext
{
    DbSet<Doctor> Doctors { get; set; }
    DbSet<Patient> Patients { get; set; }
    DbSet<MedicalHistory> MedicalHistories { get; set; }
    DbSet<Consultation> Consultations { get; set; }
    DbSet<TreatmentRoom> TreatmentRooms { get; set; }
    DbSet<TreatmentMachine> TreatmentMachines { get; set; }
    DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }

    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}