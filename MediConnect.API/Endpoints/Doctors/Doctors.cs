using Application.Doctors.UpdateAvailability;
using Carter;
using MediatR;

namespace MediConnect.API.Endpoints.Doctors;

public class Doctors : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/doctors/{doctorId:int}/availability", async (int doctorId, bool isAvailable, ISender sender) =>
        {
            try
            {
                var command = new UpdateDoctorAvailabilityCommand(doctorId, isAvailable);
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