using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class MedicalHistory : BaseEntity
{
    public int PatientId { get; private set; }
    public string Condition { get; private set; }
    public string HistoryDetails { get; private set; }

    // Relationships
    public virtual Patient Patient { get; private set; }

    private MedicalHistory() { }

    public static MedicalHistory Create(int patientId, string condition, string historyDetails)
    {
        if (string.IsNullOrWhiteSpace(condition))
            throw new DomainException("Condition cannot be empty.");

        if (string.IsNullOrWhiteSpace(historyDetails))
            throw new DomainException("History details cannot be empty.");

        return new MedicalHistory
        {
            PatientId = patientId,
            Condition = condition,
            HistoryDetails = historyDetails
        };
    }

    public void UpdateHistoryDetails(string details)
    {
        if (string.IsNullOrWhiteSpace(details))
            throw new DomainException("History details cannot be empty.");

        HistoryDetails = details;
    }

    public void ChangeCondition(string newCondition)
    {
        if (string.IsNullOrWhiteSpace(newCondition))
            throw new DomainException("New condition cannot be empty.");

        Condition = newCondition;
    }
}