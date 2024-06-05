using Domain.Entities;
using Persistence;

namespace MediConnect.API;

public static class SeedingConfiguration
{
    public static void SeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Seed Doctors
        if (!dbContext.Doctors.Any())
        {
            var doctors = new List<Doctor>
            {
                Doctor.Create(1, "Dr. Smith", "dr.smith@example.com", Specialization.Cardiologist, true),
                Doctor.Create(2, "Dr. Jones", "dr.jones@example.com", Specialization.Neurologist, true),
                Doctor.Create(3, "Dr. Green", "dr.green@example.com", Specialization.Dermatologist, true)
            };
            dbContext.Doctors.AddRange(doctors);
            dbContext.SaveChanges();
        }

        // Seed Patients and Medical Histories
        if (!dbContext.Patients.Any())
        {
            var alice = Patient.Create("Alice Johnson", "alice@example.com", DateTime.UtcNow);
            var bob = Patient.Create("Bob Smith", "bob@example.com", DateTime.UtcNow);
            dbContext.Patients.AddRange(new[] { alice, bob });
            dbContext.SaveChanges();

            var histories = new List<MedicalHistory>
            {
                MedicalHistory.Create(alice.Id, "Cardiologist", "Has a history of heart conditions."),
                MedicalHistory.Create(bob.Id, "Neurologist", "Has a history of neurological disorders.")
            };
            dbContext.MedicalHistories.AddRange(histories);
            dbContext.SaveChanges();
        }

        // Seed Treatment Rooms and Machines
        if (!dbContext.TreatmentRooms.Any())
        {
            var cardiologyRoom = TreatmentRoom.Create("Cardiology Room", "Cardiologist");
            var neurologyRoom = TreatmentRoom.Create("Neurology Room", "Neurologist");
            var dermatologyRoom = TreatmentRoom.Create("Dermatology Room", "Dermatologist");
            var generalRoom1 = TreatmentRoom.Create("General Room 1", null);
            var generalRoom2 = TreatmentRoom.Create("General Room 2", null);
            dbContext.TreatmentRooms.AddRange(new[] { cardiologyRoom, neurologyRoom, dermatologyRoom, generalRoom1, generalRoom2 });
            dbContext.SaveChanges();

            if (!dbContext.TreatmentMachines.Any())
            {
                var echocardiogramMachine = TreatmentMachine.Create("Echocardiogram", false);
                var mriScannerMachine = TreatmentMachine.Create("MRI Scanner", false);
                var dermascopeMachine = TreatmentMachine.Create("Dermascope", false);

                echocardiogramMachine.AssignToRoom(cardiologyRoom);
                mriScannerMachine.AssignToRoom(neurologyRoom);
                dermascopeMachine.AssignToRoom(dermatologyRoom);

                dbContext.TreatmentMachines.AddRange(new[] { echocardiogramMachine, mriScannerMachine, dermascopeMachine });
                dbContext.SaveChanges();
            }
        }

        dbContext.SaveChanges();
    }
}