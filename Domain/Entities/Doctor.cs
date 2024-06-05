using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class Doctor : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Specialization Specialization { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Relationships
    private readonly List<Consultation> _consultations = new();
    public IReadOnlyCollection<Consultation> Consultations => _consultations.AsReadOnly();
    public DoctorAvailability Availability { get; private set; }

    private Doctor() { }

    public static Doctor Create(int id, string name, string email, Specialization specialization, bool isAvailable)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Doctor name cannot be empty.");
        if (string.IsNullOrEmpty(email))
            throw new DomainException("Doctor email cannot be empty.");

        return new Doctor
        {
            Id = id,
            Name = name,
            Email = email,
            Specialization = specialization,
            IsAvailable = isAvailable
        };
    }

    public void UpdateAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
        Availability?.UpdateAvailability(isAvailable);
    }

    public void ReassignConsultation(Consultation consultation, Doctor newDoctor)
    {
        consultation.ReassignDoctor(newDoctor);
    }
}