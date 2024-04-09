namespace Application.Consultations.Dto;

public class ScheduleConsultationResultDto
{
    public int ConsultationId { get; set; }
    public IEnumerable<MedicalHistoryDto> MedicalHistory { get; set; }
}