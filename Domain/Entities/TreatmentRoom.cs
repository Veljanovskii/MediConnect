using Domain.Entities.Base;

namespace Domain.Entities;

public class TreatmentRoom : BaseEntity
{
    public string Name { get; set; }
    public int? TreatmentMachineId { get; set; }
    public string? RoomType { get; set; }

    // Relationships
    public virtual ICollection<Consultation> Consultations { get; set; }
    public virtual TreatmentMachine TreatmentMachine { get; set; }
}