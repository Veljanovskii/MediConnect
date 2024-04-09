using Application.Consultations.Dto;
using MediatR;

namespace Application.Consultations.ScheduleControl;

public record ScheduleControlExaminationCommand(int PreviousConsultationId) : IRequest<ScheduleConsultationResultDto>;
