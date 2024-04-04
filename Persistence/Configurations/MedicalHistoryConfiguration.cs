using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class MedicalHistoryConfiguration : IEntityTypeConfiguration<MedicalHistory>
{
    public void Configure(EntityTypeBuilder<MedicalHistory> builder)
    {
        builder.HasKey(mh => mh.Id);
        builder.Property(mh => mh.Condition).IsRequired().HasMaxLength(200);
        builder.Property(mh => mh.HistoryDetails).HasMaxLength(1000);
        builder.HasOne(mh => mh.Patient)
            .WithMany(p => p.MedicalHistories) // Assuming Patient has a collection property named MedicalHistories
            .HasForeignKey(mh => mh.PatientId);
    }
}