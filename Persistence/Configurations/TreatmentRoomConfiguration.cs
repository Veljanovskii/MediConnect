using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class TreatmentRoomConfiguration : IEntityTypeConfiguration<TreatmentRoom>
{
    public void Configure(EntityTypeBuilder<TreatmentRoom> builder)
    {
        builder.HasKey(tr => tr.Id);
        builder.Property(tr => tr.Name).IsRequired().HasMaxLength(100);
        builder.Property(tr => tr.RoomType).HasMaxLength(50);
        builder.Property(tr => tr.TreatmentMachineId).IsRequired(false);

        builder.HasOne(tr => tr.TreatmentMachine)
            .WithOne(tm => tm.TreatmentRoom)
            .HasForeignKey<TreatmentMachine>(tm => tm.TreatmentRoomId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(tr => tr.Consultations)
            .WithOne(c => c.TreatmentRoom)
            .HasForeignKey(c => c.TreatmentRoomId);
    }
}