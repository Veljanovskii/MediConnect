using Domain.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.TreatmentMachines.UpdateStatus;

public class UpdateMachineMaintenanceStatusCommandHandler : IRequestHandler<UpdateMachineMaintenanceStatusCommand, int>
{
    private readonly IRepository<TreatmentMachine> _treatmentMachineRepository;

    public UpdateMachineMaintenanceStatusCommandHandler(IRepository<TreatmentMachine> treatmentMachineRepository)
    {
        _treatmentMachineRepository = treatmentMachineRepository;
    }

    public async Task<int> Handle(UpdateMachineMaintenanceStatusCommand request, CancellationToken cancellationToken)
    {
        var machine = await _treatmentMachineRepository.GetByIdAsync(request.MachineId);
        if (machine == null)
            throw new ArgumentException("Treatment machine not found.");

        machine.SetMaintenanceStatus(request.NewStatus);
        await _treatmentMachineRepository.UpdateAsync(machine);

        return machine.Id;
    }
}
