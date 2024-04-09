using Domain.Entities.Base;

namespace Domain.Entities;

public class Consultation : BaseEntity
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public int TreatmentRoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsUrgent { get; set; } = false;

    // Relationships
    public virtual Doctor Doctor { get; set; }
    public virtual Patient Patient { get; set; }
    public virtual TreatmentRoom TreatmentRoom { get; set; }
}