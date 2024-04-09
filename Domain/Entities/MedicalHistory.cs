using Domain.Entities.Base;

namespace Domain.Entities;

public class MedicalHistory : BaseEntity
{
    public int PatientId { get; set; }
    public string Condition { get; set; }
    public string HistoryDetails { get; set; }

    // Relationships
    public virtual Patient Patient { get; set; }
}