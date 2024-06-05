using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class Patient : BaseEntity
{
    private List<MedicalHistory> _medicalHistories = new();

    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime RegistrationDate { get; private set; }

    // Relationships
    public IReadOnlyCollection<MedicalHistory> MedicalHistories => _medicalHistories.AsReadOnly();
    public virtual ICollection<Consultation> Consultations { get; private set; }

    private Patient() { }

    public static Patient Create(string name, string email, DateTime registrationDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Patient name cannot be empty.");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Patient email cannot be empty.");

        return new Patient
        {
            Name = name,
            Email = email,
            RegistrationDate = registrationDate
        };
    }

    public void AddMedicalHistory(MedicalHistory medicalHistory)
    {
        if (medicalHistory == null)
            throw new DomainException("Medical history cannot be null.");
        _medicalHistories.Add(medicalHistory);
    }
}