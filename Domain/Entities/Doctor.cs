using Domain.Entities.Base;

namespace Domain.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Specialization Specialization { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Relationships
    public virtual ICollection<Consultation> Consultations { get; set; }
    public virtual DoctorAvailability Availability { get; set; }
}