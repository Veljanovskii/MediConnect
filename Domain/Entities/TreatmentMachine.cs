using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class TreatmentMachine : BaseEntity
{
    public string Type { get; private set; }
    public bool UnderMaintenance { get; private set; }
    public int TreatmentRoomId { get; private set; }

    // Relationships
    public virtual TreatmentRoom TreatmentRoom { get; private set; }

    private TreatmentMachine() { }

    public static TreatmentMachine Create(string type, bool underMaintenance)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new DomainException("Machine type cannot be empty.");

        return new TreatmentMachine
        {
            Type = type,
            UnderMaintenance = underMaintenance
        };
    }

    public void SetMaintenanceStatus(bool isUnderMaintenance)
    {
        UnderMaintenance = isUnderMaintenance;
    }

    public void AssignToRoom(TreatmentRoom room)
    {
        if (room == null)
            throw new DomainException("Treatment room cannot be null.");

        TreatmentRoom = room;
        TreatmentRoomId = room.Id;
    }
}
