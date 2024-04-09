using Application.Consultations.Schedule;
using Application.Data;
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
        private Mock<IRepository<TreatmentMachine>> _treatmentMachineRepositoryMock;
        private Mock<IRepository<TreatmentRoom>> _treatmentRoomRepositoryMock;
        private ScheduleConsultationCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _consultationRepositoryMock = new Mock<IRepository<Consultation>>();
            _doctorRepositoryMock = new Mock<IRepository<Doctor>>();
            _medicalHistoryRepositoryMock = new Mock<IRepository<MedicalHistory>>();
            _treatmentMachineRepositoryMock = new Mock<IRepository<TreatmentMachine>>();
            _treatmentRoomRepositoryMock = new Mock<IRepository<TreatmentRoom>>();
            _handler = new ScheduleConsultationCommandHandler(_doctorRepositoryMock.Object,
                _medicalHistoryRepositoryMock.Object,
                _consultationRepositoryMock.Object,
                _treatmentMachineRepositoryMock.Object,
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

            _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(doctorId))
                .ReturnsAsync(new Doctor { Id = doctorId, IsAvailable = true, Specialization = Specialization.Cardiologist });

            var medicalHistoryData = new List<MedicalHistory>
            {
                new MedicalHistory { Condition = "Cardiologist", PatientId = patientId },
                new MedicalHistory { Condition = "Neurologist", PatientId = patientId }
            }.AsQueryable();

            var mockMedicalHistory = medicalHistoryData.BuildMock();

            _medicalHistoryRepositoryMock.Setup(repo => repo.Table)
                .Returns(mockMedicalHistory);

            _treatmentRoomRepositoryMock.Setup(repo => repo.GetByIdAsync(treatmentRoomId))
                .ReturnsAsync(new TreatmentRoom { Id = treatmentRoomId, RoomType = "Cardiologist" });

            _consultationRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Consultation>()))
                .Returns(Task.CompletedTask);

            var mockConsultationData = new List<Consultation>().AsQueryable().BuildMock();
            _consultationRepositoryMock.Setup(repo => repo.Table)
                .Returns(mockConsultationData);

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