using MediatR;

namespace Application.MedicalHistories.Add;

public record AddMedicalConditionCommand(int PatientId, string Condition, string HistoryDetails) : IRequest<int>;
