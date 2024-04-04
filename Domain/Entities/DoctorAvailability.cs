using Domain.Entities.Base;

namespace Domain.Entities;

public class DoctorAvailability : BaseEntity
{
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    // Relationships
    public virtual Doctor Doctor { get; set; }
}