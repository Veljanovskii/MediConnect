using Application.MedicalHistories.Add;
using Carter;
using MediatR;

namespace MediConnect.API.Endpoints.MedicalHistories;

public class MedicalHistories : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/medical-history", async (AddMedicalConditionCommand command, ISender sender) =>
        {
            try
            {
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