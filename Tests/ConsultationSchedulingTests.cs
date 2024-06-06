using Application.Consultations.Schedule;
using Domain.Interfaces;
using Domain.Entities;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ConsultationSchedulingTests
    {
        private Mock<IRepository<Consultation>> _consultationRepositoryMock;
        private Mock<IRepository<Doctor>> _doctorRepositoryMock;
        private Mock<IRepository<MedicalHistory>> _medicalHistoryRepositoryMock;
        private Mock<IRepository<Patient>> _patientRepositoryMock;
        private Mock<IRepository<TreatmentRoom>> _treatmentRoomRepositoryMock;
        private ScheduleConsultationCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _consultationRepositoryMock = new Mock<IRepository<Consultation>>();
            _doctorRepositoryMock = new Mock<IRepository<Doctor>>();
            _medicalHistoryRepositoryMock = new Mock<IRepository<MedicalHistory>>();
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _treatmentRoomRepositoryMock = new Mock<IRepository<TreatmentRoom>>();
            _handler = new ScheduleConsultationCommandHandler(_doctorRepositoryMock.Object,
                _medicalHistoryRepositoryMock.Object,
                _consultationRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _treatmentRoomRepositoryMock.Object);
        }

        [Test]
        public async Task ScheduleConsultation_ShouldCreateConsultation()
        {
            // Arrange
            var doctorId = 1;
            var patientId = 1;
            var treatmentRoomId = 1;
            var startTime = DateTime.UtcNow.AddHours(1);
            var isUrgent = false;

            var command = new ScheduleConsultationCommand(doctorId, patientId, treatmentRoomId, startTime, isUrgent);

            var doctor = Doctor.Create(doctorId, "Dr. Cardio", "cardio@example.com", Specialization.Cardiologist, true);
            _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            var patient = Patient.Create("Alice Johnson", "alice@example.com", DateTime.UtcNow);
            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var cardiologyHistory = MedicalHistory.Create(patientId, "Cardiologist", "Has a history of heart conditions.");
            var neurologyHistory = MedicalHistory.Create(patientId, "Neurologist", "Has a history of neurological disorders.");

            patient.AddMedicalHistory(cardiologyHistory);
            patient.AddMedicalHistory(neurologyHistory);

            var medicalHistoryData = new List<MedicalHistory>
            {
                cardiologyHistory,
                neurologyHistory
            }.AsQueryable();

            //var mockMedicalHistory = medicalHistoryData.BuildMock();
            //_medicalHistoryRepositoryMock.Setup(repo => repo.Table)
            //    .Returns(mockMedicalHistory);

            var treatmentRoom = TreatmentRoom.Create("Cardiology Room", "Cardiologist");
            _treatmentRoomRepositoryMock.Setup(repo => repo.GetByIdAsync(treatmentRoomId)).ReturnsAsync(treatmentRoom);

            //var mockConsultationData = new List<Consultation>().AsQueryable().BuildMock();
            //_consultationRepositoryMock.Setup(repo => repo.Table)
            //    .Returns(mockConsultationData);

            _consultationRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Consultation>()))
                .Callback((Consultation consultation) =>
                {
                    consultation.Id = 1;
                })
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ConsultationId, Is.GreaterThan(0));
            Assert.That(result.MedicalHistory, Is.Not.Empty);
            _consultationRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Consultation>()), Times.Once);
        }
    }
}