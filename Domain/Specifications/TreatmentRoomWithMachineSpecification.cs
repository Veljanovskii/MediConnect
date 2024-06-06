using Domain.Entities;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specifications;

public class TreatmentRoomWithMachineSpecification : ISpecification<TreatmentRoom>
{
    public Expression<Func<TreatmentRoom, bool>> Criteria { get; }
    public List<Expression<Func<TreatmentRoom, object>>> Includes { get; } = new List<Expression<Func<TreatmentRoom, object>>>();

    public List<string> IncludeStrings { get; } = new List<string>();

    public TreatmentRoomWithMachineSpecification(int treatmentRoomId)
    {
        Criteria = room => room.Id == treatmentRoomId;
        Includes.Add(room => room.TreatmentMachine);
    }
}