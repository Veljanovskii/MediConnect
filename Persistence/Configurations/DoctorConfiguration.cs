using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired();
        builder.Property(d => d.Email).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Specialization).HasConversion<string>();
        builder.Property(d => d.IsAvailable);
        builder.HasMany(d => d.Consultations).WithOne(c => c.Doctor).HasForeignKey(c => c.DoctorId);
        builder.HasOne(d => d.Availability).WithOne(a => a.Doctor).HasForeignKey<DoctorAvailability>(a => a.DoctorId);
    }
}