from sqlalchemy import Column, Integer, String, ForeignKey, DateTime, Boolean, Enum
from sqlalchemy.orm import relationship
from app.core.database import Base  # Assuming you have a database.py with Base defined
import enum


class Specialization(enum.Enum):
    cardiologist = "cardiologist"
    neurologist = "neurologist"
    dermatologist = "dermatologist"


class Doctor(Base):
    __tablename__ = "doctors"

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    email = Column(String, unique=True, index=True)
    specialization = Column(Enum(Specialization))
    is_available = Column(Boolean, default=True)

    # Relationships
    consultations = relationship("Consultation", back_populates="doctor")
    availability = relationship("DoctorAvailability", back_populates="doctor")


class Patient(Base):
    __tablename__ = "patients"

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    email = Column(String, unique=True, index=True)
    registration_date = Column(DateTime, index=True)

    # Relationships
    medical_history = relationship("MedicalHistory", back_populates="patient")
    consultations = relationship("Consultation", back_populates="patient")


class MedicalHistory(Base):
    __tablename__ = "medical_histories"

    id = Column(Integer, primary_key=True, index=True)
    patient_id = Column(Integer, ForeignKey("patients.id"))
    condition = Column(String)
    history_details = Column(String)

    # Relationships
    patient = relationship("Patient", back_populates="medical_history")


class Consultation(Base):
    __tablename__ = "consultations"

    id = Column(Integer, primary_key=True, index=True)
    doctor_id = Column(Integer, ForeignKey("doctors.id"))
    patient_id = Column(Integer, ForeignKey("patients.id"))
    treatment_room_id = Column(Integer, ForeignKey("treatment_rooms.id"))
    start_time = Column(DateTime)
    end_time = Column(DateTime)    
    is_urgent = Column(Boolean, default=False)  # New field

    # Relationships
    doctor = relationship("Doctor", back_populates="consultations")
    patient = relationship("Patient", back_populates="consultations")
    treatment_room = relationship("TreatmentRoom", back_populates="consultations")


class TreatmentRoom(Base):
    __tablename__ = "treatment_rooms"

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    treatment_machine_id = Column(Integer, ForeignKey("treatment_machines.id"))
    room_type = Column(String, index=True)  # New column for room type

    # Relationships
    consultations = relationship("Consultation", back_populates="treatment_room")
    treatment_machine = relationship("TreatmentMachine", back_populates="treatment_room")


class TreatmentMachine(Base):
    __tablename__ = "treatment_machines"

    id = Column(Integer, primary_key=True, index=True)
    type = Column(String, index=True)
    under_maintenance = Column(Boolean, default=False)

    # Relationships
    treatment_room = relationship("TreatmentRoom", back_populates="treatment_machine")


class DoctorAvailability(Base):
    __tablename__ = "doctor_availabilities"

    id = Column(Integer, primary_key=True, index=True)
    doctor_id = Column(Integer, ForeignKey("doctors.id"))
    date = Column(DateTime)
    start_time = Column(DateTime)
    end_time = Column(DateTime)
    status = Column(String)

    # Relationships
    doctor = relationship("Doctor", back_populates="availability")
