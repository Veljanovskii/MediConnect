using Application.Consultations.Dto;
using Application.Data;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Consultations.Schedule;

internal class ScheduleConsultationCommandHandler : IRequestHandler<ScheduleConsultationCommand, ScheduleConsultationResultDto>
{
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IRepository<MedicalHistory> _medicalHistoryRepository;
    private readonly IRepository<Consultation> _consultationRepository;
    private readonly IRepository<TreatmentMachine> _treatmentMachineRepository;
    private readonly IRepository<TreatmentRoom> _treatmentRoomRepository;

    public ScheduleConsultationCommandHandler(
        IRepository<Doctor> doctorRepository,
        IRepository<MedicalHistory> medicalHistoryRepository,
        IRepository<Consultation> consultationRepository,
        IRepository<TreatmentMachine> treatmentMachineRepository,
        IRepository<TreatmentRoom> treatmentRoomRepository)
    {
        _doctorRepository = doctorRepository;
        _medicalHistoryRepository = medicalHistoryRepository;
        _consultationRepository = consultationRepository;
        _treatmentMachineRepository = treatmentMachineRepository;
        _treatmentRoomRepository = treatmentRoomRepository;
    }

    public async Task<ScheduleConsultationResultDto> Handle(ScheduleConsultationCommand request, CancellationToken cancellationToken)
    {
        var roundedStartTime = new DateTime(
            request.StartTime.Year,
            request.StartTime.Month,
            request.StartTime.Day,
            request.StartTime.Hour,
            0, 0, DateTimeKind.Utc);
        var endTime = roundedStartTime.AddHours(1);

        // Check if the doctor is available
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
            throw new ArgumentException("Doctor not found.");
        if (!doctor.IsAvailable)
            throw new ArgumentException("Doctor is not available for consultation.");

        // Check if the time slot is available, unless the consultation is urgent
        if (!request.IsUrgent && !await IsTimeSlotAvailable(request.DoctorId, roundedStartTime, endTime))
            throw new ArgumentException("The time slot is not available.");

        // Check if the doctor's specialization matches the patient's medical condition
        var patientConditions = _medicalHistoryRepository.Table
            .Where(mh => mh.PatientId == request.PatientId)
            .Select(mh => mh.Condition)
            .ToList();

        if (!patientConditions.Contains(doctor.Specialization.ToString()))
            throw new ArgumentException("Doctor's specialization does not match patient's medical condition.");

        // Check if the treatment room matches the doctor's specialization
        var treatmentRoom = await _treatmentRoomRepository.GetByIdAsync(request.TreatmentRoomId);
        if (treatmentRoom == null)
            throw new ArgumentException("Treatment room not found.");
        if (treatmentRoom.RoomType != null && treatmentRoom.RoomType != doctor.Specialization.ToString())
            throw new ArgumentException("Treatment room does not match doctor's specialization.");

        // Check if the treatment room's machine is under maintenance
        if (treatmentRoom.TreatmentMachineId.HasValue)
        {
            var machine = await _treatmentMachineRepository.GetByIdAsync(treatmentRoom.TreatmentMachineId.Value);
            if (machine == null)
                throw new ArgumentException("Treatment room's machine not found.");
            if (machine.UnderMaintenance)
                throw new ArgumentException("Treatment room's machine is under maintenance.");
        }

        var newConsultation = new Consultation
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            TreatmentRoomId = request.TreatmentRoomId,
            StartTime = roundedStartTime,
            EndTime = endTime,
            IsUrgent = request.IsUrgent
        };

        await _consultationRepository.InsertAsync(newConsultation);

        var medicalHistories = await _medicalHistoryRepository.Table
            .Where(mh => mh.PatientId == request.PatientId)
            .Select(mh => new MedicalHistoryDto
            {
                PatientId = mh.PatientId,
                Condition = mh.Condition,
                HistoryDetails = mh.HistoryDetails
            })
            .ToListAsync();

        return new ScheduleConsultationResultDto
        {
            ConsultationId = newConsultation.Id,
            MedicalHistory = medicalHistories
        };
    }

    private async Task<bool> IsTimeSlotAvailable(int doctorId, DateTime startTime, DateTime endTime)
    {
        var overlappingConsultations = await _consultationRepository.Table
            .Where(c => c.DoctorId == doctorId &&
                         c.EndTime > startTime &&
                         c.StartTime < endTime)
            .FirstOrDefaultAsync();

        return overlappingConsultations == null;
    }
}