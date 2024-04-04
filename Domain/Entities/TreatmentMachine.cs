using Domain.Entities.Base;

namespace Domain.Entities;

public class TreatmentMachine : BaseEntity
{
    public string Type { get; set; }
    public bool UnderMaintenance { get; set; } = false;
    public int TreatmentRoomId { get; set; }

    // Relationships
    public virtual TreatmentRoom TreatmentRoom { get; set; }
}