using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specifications;

public class MedicalHistoriesByPatientIdSpecification : ISpecification<MedicalHistory>
{
    public Expression<Func<MedicalHistory, bool>> Criteria { get; }

    public List<Expression<Func<MedicalHistory, object>>> Includes { get; } = new List<Expression<Func<MedicalHistory, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();

    public MedicalHistoriesByPatientIdSpecification(int patientId)
    {
        Criteria = mh => mh.PatientId == patientId;
    }
}
