using Application.Consultations.Dto;
using MediatR;

namespace Application.Consultations.Schedule;

public record ScheduleConsultationCommand(
    int DoctorId,
    int PatientId,
    int TreatmentRoomId,
    DateTime StartTime,
    bool IsUrgent) : IRequest<ScheduleConsultationResultDto>;