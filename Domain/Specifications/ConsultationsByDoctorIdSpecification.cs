using Domain.Interfaces;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Specifications;

public class ConsultationsByDoctorIdSpecification : ISpecification<Consultation>
{
    public Expression<Func<Consultation, bool>> Criteria { get; }

    public List<Expression<Func<Consultation, object>>> Includes { get; } = new List<Expression<Func<Consultation, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();

    public ConsultationsByDoctorIdSpecification(int doctorId)
    {
        Criteria = consultation => consultation.DoctorId == doctorId;
    }
}
