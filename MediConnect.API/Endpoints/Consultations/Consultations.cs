using Application.Consultations.Schedule;
using Application.Consultations.ScheduleControl;
using Carter;
using MediatR;

namespace MediConnect.API.Endpoints.Consultations;

public class Consultations : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/schedule-consultation", async (ScheduleConsultationCommand command, ISender sender) =>
        {
            try
            {
                var resultDto = await sender.Send(command);
                return Results.Ok(resultDto);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });

        app.MapPost("/schedule-control-examination/{previousConsultationId:int}", async (int previousConsultationId, ISender sender) =>
        {
            try
            {
                var command = new ScheduleControlExaminationCommand(previousConsultationId);
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