using Application.Consultations.Dto;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Consultations.ScheduleControl;

public class ScheduleControlExaminationCommandHandler : IRequestHandler<ScheduleControlExaminationCommand, ScheduleConsultationResultDto>
{
    private readonly IRepository<Consultation> _consultationRepository;
    private readonly IRepository<MedicalHistory> _medicalHistoryRepository;

    public ScheduleControlExaminationCommandHandler(IRepository<Consultation> consultationRepository, IRepository<MedicalHistory> medicalHistoryRepository)
    {
        _consultationRepository = consultationRepository;
        _medicalHistoryRepository = medicalHistoryRepository;
    }

    public async Task<ScheduleConsultationResultDto> Handle(ScheduleControlExaminationCommand request, CancellationToken cancellationToken)
    {
        var previousConsultation = await _consultationRepository.GetByIdAsync(request.PreviousConsultationId);
        if (previousConsultation == null)
            throw new ArgumentException("Previous consultation not found");

        var controlExaminationStartTime = previousConsultation.StartTime.AddDays(14);
        var roundedStartTime = new DateTime(controlExaminationStartTime.Year, controlExaminationStartTime.Month, controlExaminationStartTime.Day, controlExaminationStartTime.Hour, 0, 0, DateTimeKind.Utc);

        var controlExamination = Consultation.ScheduleControlExamination(previousConsultation, roundedStartTime);

        await _consultationRepository.InsertAsync(controlExamination);

        var medicalHistories = await _medicalHistoryRepository.Table
            .Where(mh => mh.PatientId == controlExamination.PatientId)
            .Select(mh => new MedicalHistoryDto
            {
                PatientId = mh.PatientId,
                Condition = mh.Condition,
                HistoryDetails = mh.HistoryDetails
            })
            .ToListAsync();

        return new ScheduleConsultationResultDto
        {
            ConsultationId = controlExamination.Id,
            MedicalHistory = medicalHistories
        };
    }
}
