using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class DoctorAvailabilityConfiguration : IEntityTypeConfiguration<DoctorAvailability>
{
    public void Configure(EntityTypeBuilder<DoctorAvailability> builder)
    {
        builder.HasKey(da => da.Id);
        builder.Property(da => da.Date).IsRequired();
        builder.Property(da => da.StartTime).IsRequired();
        builder.Property(da => da.EndTime).IsRequired();
        builder.Property(da => da.Status).IsRequired().HasMaxLength(50);
        builder.HasOne(da => da.Doctor).WithOne(d => d.Availability).HasForeignKey<Doctor>(d => d.Id);
    }
}