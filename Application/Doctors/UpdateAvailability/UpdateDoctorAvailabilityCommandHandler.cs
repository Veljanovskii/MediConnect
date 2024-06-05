using Domain.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Specifications;
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
            throw new DomainException("Doctor not found");

        doctor.UpdateAvailability(request.IsAvailable);
        await _doctorRepository.UpdateAsync(doctor);

        if (!request.IsAvailable)
        {
            await HandleUnavailableDoctor(doctor);
        }

        return doctor.Id;
    }

    private async Task HandleUnavailableDoctor(Doctor doctor)
    {
        var consultationsToReassign = doctor.Consultations
            .Where(c => c.StartTime > DateTime.UtcNow)
            .ToList();

        foreach (var consultation in consultationsToReassign)
        {
            var spec = new DoctorAvailableBySpecializationSpecification(consultation.Doctor.Specialization);
            var newDoctor = await _doctorRepository.GetAsync(spec);

            if (newDoctor != null)
            {
                doctor.ReassignConsultation(consultation, newDoctor);
                await _consultationRepository.UpdateAsync(consultation);
                NotifyPatientReassignment(consultation.PatientId);
            }
            else
            {
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
