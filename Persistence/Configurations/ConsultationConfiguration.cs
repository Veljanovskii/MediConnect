using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.StartTime).IsRequired();
        builder.Property(c => c.EndTime).IsRequired();
        builder.Property(c => c.IsUrgent);
        builder.HasOne(c => c.Doctor).WithMany(d => d.Consultations).HasForeignKey(c => c.DoctorId);
        builder.HasOne(c => c.Patient).WithMany(p => p.Consultations).HasForeignKey(c => c.PatientId);
        builder.HasOne(c => c.TreatmentRoom).WithMany(tr => tr.Consultations).HasForeignKey(c => c.TreatmentRoomId);
    }
}