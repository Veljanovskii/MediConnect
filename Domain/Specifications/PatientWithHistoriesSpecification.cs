using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specifications;

public class PatientWithHistoriesSpecification : ISpecification<Patient>
{
    public Expression<Func<Patient, bool>> Criteria { get; }

    public List<Expression<Func<Patient, object>>> Includes { get; } = new List<Expression<Func<Patient, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();

    public PatientWithHistoriesSpecification(int patientId)
    {
        Criteria = patient => patient.Id == patientId;
        Includes.Add(patient => patient.MedicalHistories);
    }
}
