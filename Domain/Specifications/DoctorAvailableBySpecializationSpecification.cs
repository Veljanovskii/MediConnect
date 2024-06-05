using Domain.Interfaces;
using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Specifications;

public class DoctorAvailableBySpecializationSpecification : ISpecification<Doctor>
{
    public Expression<Func<Doctor, bool>> Criteria =>
        doctor => doctor.IsAvailable && doctor.Specialization == _specialization;

    public List<Expression<Func<Doctor, object>>> Includes => new List<Expression<Func<Doctor, object>>>();
    public List<string> IncludeStrings => new List<string>();

    private readonly Specialization _specialization;

    public DoctorAvailableBySpecializationSpecification(Specialization specialization)
    {
        _specialization = specialization;
    }
}
