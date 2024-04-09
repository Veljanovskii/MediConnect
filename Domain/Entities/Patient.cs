using Domain.Entities.Base;

namespace Domain.Entities;

public class Patient : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }

    // Relationships
    public ICollection<MedicalHistory> MedicalHistories { get; set; }
    public virtual ICollection<Consultation> Consultations { get; set; }
}