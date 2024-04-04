using MediatR;

namespace Application.Doctors.UpdateAvailability;

public record UpdateDoctorAvailabilityCommand(int DoctorId, bool IsAvailable) : IRequest<int>;
