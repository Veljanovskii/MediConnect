using MediatR;

namespace Application.TreatmentMachines.UpdateStatus;

public record UpdateMachineMaintenanceStatusCommand(int MachineId, bool NewStatus) : IRequest<int>;
