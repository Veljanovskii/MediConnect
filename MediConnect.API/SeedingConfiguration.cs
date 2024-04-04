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
            dbContext.Doctors.AddRange(
                new Doctor { Name = "Dr. Smith", Email = "dr.smith@example.com", Specialization = Specialization.Cardiologist },
                new Doctor { Name = "Dr. Jones", Email = "dr.jones@example.com", Specialization = Specialization.Neurologist },
                new Doctor { Name = "Dr. Green", Email = "dr.green@example.com", Specialization = Specialization.Dermatologist }
            );
        }

        // Seed Patients
        if (!dbContext.Patients.Any())
        {
            var patients = new List<Patient>
            {
                new() { Name = "Alice Johnson", Email = "alice@example.com", RegistrationDate = DateTime.UtcNow },
                new() { Name = "Bob Smith", Email = "bob@example.com", RegistrationDate = DateTime.UtcNow }
            };
            dbContext.Patients.AddRange(patients);
            dbContext.SaveChanges();

            // Seed Medical Histories using actual Patient IDs
            if (!dbContext.MedicalHistories.Any())
            {
                dbContext.MedicalHistories.AddRange(
                    new MedicalHistory { PatientId = patients[0].Id, Condition = "Cardiologist", HistoryDetails = "Has a history of heart conditions." },
                    new MedicalHistory { PatientId = patients[1].Id, Condition = "Neurologist", HistoryDetails = "Has a history of neurological disorders." }
                );
                dbContext.SaveChanges();
            }
        }

        // Seed Treatment Rooms
        if (!dbContext.TreatmentRooms.Any())
        {
            var cardiologyRoom = new TreatmentRoom { Name = "Cardiology Room", RoomType = "Cardiologist" };
            var neurologyRoom = new TreatmentRoom { Name = "Neurology Room", RoomType = "Neurologist" };
            var dermatologyRoom = new TreatmentRoom { Name = "Dermatology Room", RoomType = "Dermatologist" };
            var generalRoom1 = new TreatmentRoom { Name = "General Room 1" };
            var generalRoom2 = new TreatmentRoom { Name = "General Room 2" };

            dbContext.TreatmentRooms.AddRange(cardiologyRoom, neurologyRoom, dermatologyRoom, generalRoom1, generalRoom2);
            dbContext.SaveChanges();

            // Seed Treatment Machines with Room IDs
            if (!dbContext.TreatmentMachines.Any())
            {
                var echocardiogramMachine = new TreatmentMachine { Type = "Echocardiogram", UnderMaintenance = false, TreatmentRoom = cardiologyRoom };
                var mriScannerMachine = new TreatmentMachine { Type = "MRI Scanner", UnderMaintenance = false, TreatmentRoom = neurologyRoom };
                var dermascopeMachine = new TreatmentMachine { Type = "Dermascope", UnderMaintenance = false, TreatmentRoom = dermatologyRoom };

                dbContext.TreatmentMachines.AddRange(echocardiogramMachine, mriScannerMachine, dermascopeMachine);
                dbContext.SaveChanges();

                // Update Treatment Rooms with Machine IDs
                cardiologyRoom.TreatmentMachineId = echocardiogramMachine.Id;
                neurologyRoom.TreatmentMachineId = mriScannerMachine.Id;
                dermatologyRoom.TreatmentMachineId = dermascopeMachine.Id;

                dbContext.SaveChanges();
            }
        }

        dbContext.SaveChanges();
    }
}