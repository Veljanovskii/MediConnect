using Domain.Entities.Base;

namespace Domain.Entities;

public class DoctorAvailability : BaseEntity
{
    public int DoctorId { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Status { get; private set; }

    // Relationships
    public virtual Doctor Doctor { get; private set; }

    private DoctorAvailability() { }

    public static DoctorAvailability Create(int doctorId, DateTime date, DateTime startTime, DateTime endTime, string status)
    {
        return new DoctorAvailability
        {
            DoctorId = doctorId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Status = status
        };
    }

    public void UpdateAvailability(bool isAvailable)
    {
        Status = isAvailable ? "Available" : "Unavailable";
    }
}