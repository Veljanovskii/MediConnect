using Application.TreatmentMachines.UpdateStatus;
using Carter;
using MediatR;

namespace MediConnect.API.Endpoints.TreatmentMachines;

public class TreatmentMachines : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/machines/{machineId:int}/maintenance-status", async (int machineId, bool newStatus, ISender sender) =>
        {
            try
            {
                var command = new UpdateMachineMaintenanceStatusCommand(machineId, newStatus);
                var result = await sender.Send(command);
                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
    }
}