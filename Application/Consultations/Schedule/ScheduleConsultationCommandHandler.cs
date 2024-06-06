using Application.Consultations.Dto;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using Domain.Specifications;

namespace Application.Consultations.Schedule;

public class ScheduleConsultationCommandHandler : IRequestHandler<ScheduleConsultationCommand, ScheduleConsultationResultDto>
{
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IRepository<MedicalHistory> _medicalHistoryRepository;
    private readonly IRepository<Consultation> _consultationRepository;
    private readonly IRepository<Patient> _patientRepository;
    private readonly IRepository<TreatmentRoom> _treatmentRoomRepository;

    public ScheduleConsultationCommandHandler(
        IRepository<Doctor> doctorRepository,
        IRepository<MedicalHistory> medicalHistoryRepository,
        IRepository<Consultation> consultationRepository,
        IRepository<Patient> patientRepository,
        IRepository<TreatmentRoom> treatmentRoomRepository)
    {
        _doctorRepository = doctorRepository;
        _medicalHistoryRepository = medicalHistoryRepository;
        _consultationRepository = consultationRepository;
        _patientRepository = patientRepository;
        _treatmentRoomRepository = treatmentRoomRepository;
    }

    public async Task<ScheduleConsultationResultDto> Handle(ScheduleConsultationCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        var patientSpec = new PatientWithHistoriesSpecification(request.PatientId);
        var patient = await _patientRepository.GetAsync(patientSpec);
        var roomSpec = new TreatmentRoomWithMachineSpecification(request.TreatmentRoomId);
        var treatmentRoom = await _treatmentRoomRepository.GetAsync(roomSpec);
        var spec = new ConsultationsByDoctorIdSpecification(request.DoctorId);
        var existingConsultations = await _consultationRepository.ListAsync(spec);
        var roundedStartTime = new DateTime(request.StartTime.Year, request.StartTime.Month, request.StartTime.Day, request.StartTime.Hour, 0, 0, DateTimeKind.Utc);

        var newConsultation = Consultation.Create(
            request.DoctorId, request.PatientId, request.TreatmentRoomId,
            roundedStartTime, request.IsUrgent, doctor, patient, treatmentRoom, existingConsultations);

        await _consultationRepository.InsertAsync(newConsultation);

        var medicalHistorySpec = new MedicalHistoriesByPatientIdSpecification(request.PatientId);
        var medicalHistories = await _medicalHistoryRepository.ListAsync(medicalHistorySpec);

        return new ScheduleConsultationResultDto
        {
            ConsultationId = newConsultation.Id,
            MedicalHistory = medicalHistories.Select(mh => new MedicalHistoryDto
            {
                PatientId = mh.PatientId,
                Condition = mh.Condition,
                HistoryDetails = mh.HistoryDetails
            }).ToList()
        };
    }
}