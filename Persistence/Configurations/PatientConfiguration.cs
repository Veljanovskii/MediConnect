using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
        builder.Property(p => p.RegistrationDate).IsRequired();
        builder.HasMany(p => p.Consultations).WithOne(c => c.Patient).HasForeignKey(c => c.PatientId);
        builder.HasMany(p => p.MedicalHistories)
            .WithOne(mh => mh.Patient)
            .HasForeignKey(mh => mh.PatientId);
    }
}