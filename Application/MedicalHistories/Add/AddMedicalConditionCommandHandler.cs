using Domain.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.MedicalHistories.Add;

public class AddMedicalConditionCommandHandler : IRequestHandler<AddMedicalConditionCommand, int>
{
    private readonly IRepository<MedicalHistory> _medicalHistoryRepository;

    public AddMedicalConditionCommandHandler(IRepository<MedicalHistory> medicalHistoryRepository)
    {
        _medicalHistoryRepository = medicalHistoryRepository;
    }

    public async Task<int> Handle(AddMedicalConditionCommand request, CancellationToken cancellationToken)
    {
        var newMedicalHistory = MedicalHistory.Create(
            request.PatientId,
            request.Condition,
            request.HistoryDetails
        );

        await _medicalHistoryRepository.InsertAsync(newMedicalHistory);

        return newMedicalHistory.Id;
    }
}
