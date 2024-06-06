using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class Consultation : BaseEntity
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public int TreatmentRoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsUrgent { get; set; } = false;

    // Relationships
    public virtual Doctor Doctor { get; set; }
    public virtual Patient Patient { get; set; }
    public virtual TreatmentRoom TreatmentRoom { get; set; }

    private Consultation() { }

    public static Consultation Create(int doctorId, int patientId, int treatmentRoomId, DateTime startTime, bool isUrgent,
                                      Doctor doctor, Patient patient, TreatmentRoom treatmentRoom, IEnumerable<Consultation> existingConsultations)
    {
        ValidateDoctorAvailability(doctor);
        ValidateRoomAvailability(treatmentRoom);
        ValidateRoomMatchDoctorSpecialization(treatmentRoom, doctor);
        ValidateTimeSlot(startTime, doctorId, existingConsultations, isUrgent);
        ValidateSpecialization(doctor, patient);

        return new Consultation
        {
            DoctorId = doctorId,
            PatientId = patientId,
            TreatmentRoomId = treatmentRoomId,
            StartTime = startTime,
            EndTime = startTime.AddHours(1),
            IsUrgent = isUrgent,
            Doctor = doctor,
            Patient = patient,
            TreatmentRoom = treatmentRoom
        };
    }

    private static void ValidateDoctorAvailability(Doctor doctor)
    {
        if (doctor == null || !doctor.IsAvailable)
            throw new DomainException("Doctor is not available or does not exist.");
    }

    private static void ValidateRoomAvailability(TreatmentRoom room)
    {
        if (room == null || (room.TreatmentMachine != null && room.TreatmentMachine.UnderMaintenance))
            throw new DomainException("Treatment room is not available or its machine is under maintenance.");
    }

    private static void ValidateRoomMatchDoctorSpecialization(TreatmentRoom room, Doctor doctor)
    {
        if (room.RoomType != null && room.RoomType != doctor.Specialization.ToString())
            throw new DomainException("Treatment room does not match doctor's specialization.");
    }

    private static void ValidateTimeSlot(DateTime startTime, int doctorId, IEnumerable<Consultation> existingConsultations, bool isUrgent)
    {
        if (!isUrgent)
        {
            var endTime = startTime.AddHours(1);
            var isAvailable = existingConsultations.All(c => c.DoctorId != doctorId || c.EndTime <= startTime || c.StartTime >= endTime);
            if (!isAvailable)
                throw new DomainException("The time slot is not available.");
        }
    }

    private static void ValidateSpecialization(Doctor doctor, Patient patient)
    {
        var patientConditions = patient.MedicalHistories.Select(mh => mh.Condition);
        if (!patientConditions.Contains(doctor.Specialization.ToString()))
            throw new DomainException("Doctor's specialization does not match patient's medical condition.");
    }

    public void ReassignDoctor(Doctor newDoctor)
    {
        if (newDoctor == null)
            throw new DomainException("New doctor cannot be null.");

        if (newDoctor.Specialization != Doctor.Specialization)
            throw new DomainException("Cannot reassign consultation to a doctor with a different specialization.");

        DoctorId = newDoctor.Id;
        Doctor = newDoctor;
    }

    public static Consultation ScheduleControlExamination(Consultation previousConsultation, DateTime startTime)
    {
        if (previousConsultation == null)
            throw new DomainException("Previous consultation cannot be null.");

        if (startTime.AddHours(1) < previousConsultation.StartTime.AddDays(14))
            throw new DomainException("Control examination must be scheduled at least two weeks after the initial consultation.");

        return new Consultation
        {
            DoctorId = previousConsultation.DoctorId,
            PatientId = previousConsultation.PatientId,
            TreatmentRoomId = previousConsultation.TreatmentRoomId,
            StartTime = startTime,
            EndTime = startTime.AddHours(1),
            IsUrgent = false,
            Doctor = previousConsultation.Doctor,
            Patient = previousConsultation.Patient,
            TreatmentRoom = previousConsultation.TreatmentRoom
        };
    }
}