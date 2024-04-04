using Application.Data;
using Domain.Entities;
using MediatR;

namespace Application.Doctors.UpdateAvailability;

public class UpdateDoctorAvailabilityCommandHandler : IRequestHandler<UpdateDoctorAvailabilityCommand, int>
{
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IRepository<Consultation> _consultationRepository;

    public UpdateDoctorAvailabilityCommandHandler(IRepository<Doctor> doctorRepository, IRepository<Consultation> consultationRepository)
    {
        _doctorRepository = doctorRepository;
        _consultationRepository = consultationRepository;
    }

    public async Task<int> Handle(UpdateDoctorAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
            throw new ArgumentException("Doctor not found");

        doctor.IsAvailable = request.IsAvailable;
        await _doctorRepository.UpdateAsync(doctor);

        if (!request.IsAvailable)
        {
            await HandleUnavailableDoctor(doctor.Id);
        }
        
        return doctor.Id;
    }

    private async Task HandleUnavailableDoctor(int doctorId)
    {
        var consultations = _consultationRepository.Table
            .Where(c => c.DoctorId == doctorId && c.StartTime > DateTime.UtcNow)
            .ToList();

        foreach (var consultation in consultations)
        {
            // Find a new available doctor with the same specialization
            var newDoctor = _doctorRepository.Table
                .FirstOrDefault(d => d.Specialization == consultation.Doctor.Specialization && d.IsAvailable);

            if (newDoctor != null)
            {
                consultation.DoctorId = newDoctor.Id;
                await _consultationRepository.UpdateAsync(consultation);

                // Notify the patient about the reassignment
                NotifyPatientReassignment(consultation.PatientId);
            }
            else
            {
                // Notify the patient that their consultation needs to be rescheduled
                NotifyPatientReschedule(consultation.PatientId);
            }
        }
    }

    private void NotifyPatientReassignment(int patientId)
    {
        Console.WriteLine($"Patient with ID {patientId} has been reassigned to a new doctor.");
    }

    private void NotifyPatientReschedule(int patientId)
    {
        Console.WriteLine($"Patient with ID {patientId} needs to reschedule their consultation.");
    }
}
