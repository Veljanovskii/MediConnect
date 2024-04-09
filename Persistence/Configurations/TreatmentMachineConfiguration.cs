using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class TreatmentMachineConfiguration : IEntityTypeConfiguration<TreatmentMachine>
{
    public void Configure(EntityTypeBuilder<TreatmentMachine> builder)
    {
        builder.HasKey(tm => tm.Id);
        builder.Property(tm => tm.Type).IsRequired().HasMaxLength(100);
        builder.Property(tm => tm.UnderMaintenance);
        builder.HasOne(tm => tm.TreatmentRoom).WithOne(tr => tr.TreatmentMachine).HasForeignKey<TreatmentRoom>(tr => tr.Id);
    }
}