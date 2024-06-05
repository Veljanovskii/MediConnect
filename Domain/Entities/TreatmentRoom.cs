using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class TreatmentRoom : BaseEntity
{
    public string Name { get; private set; }
    public int? TreatmentMachineId { get; private set; }
    public string? RoomType { get; private set; }

    // Relationships
    public virtual ICollection<Consultation> Consultations { get; private set; } = new List<Consultation>();
    public virtual TreatmentMachine TreatmentMachine { get; private set; }

    private TreatmentRoom() { }

    public static TreatmentRoom Create(string name, string roomType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Room name cannot be empty.");

        return new TreatmentRoom
        {
            Name = name,
            RoomType = roomType
        };
    }

    public void AssignMachine(TreatmentMachine machine)
    {
        if (machine == null)
            throw new DomainException("Machine cannot be null.");

        if (machine.UnderMaintenance)
            throw new DomainException("Cannot assign a machine that is under maintenance.");

        TreatmentMachine = machine;
        TreatmentMachineId = machine.Id;
    }

    public void RemoveMachine()
    {
        TreatmentMachine = null;
        TreatmentMachineId = null;
    }
}
