from sqlalchemy.orm import Session
from app.core.database import SessionLocal, engine
from .models import Base, Doctor, Patient, MedicalHistory, TreatmentRoom, TreatmentMachine, Specialization
from app.core.config import settings
import datetime

# Create tables if they don't exist yet (optional, remove if you have another migration strategy)
Base.metadata.create_all(bind=engine)

def seed_data():
    db = SessionLocal()

    # Seed Doctors
    if db.query(Doctor).count() == 0:
        db.add_all([
            Doctor(name="Dr. Smith", email="dr.smith@example.com", specialization=Specialization.cardiologist),
            Doctor(name="Dr. Jones", email="dr.jones@example.com", specialization=Specialization.neurologist),
            Doctor(name="Dr. Green", email="dr.green@example.com", specialization=Specialization.dermatologist),
        ])
        db.commit()

    # Seed Patients
    if db.query(Patient).count() == 0:
        db.add_all([
            Patient(name="Alice Johnson", email="alice@example.com", registration_date=datetime.datetime.now()),
            Patient(name="Bob Smith", email="bob@example.com", registration_date=datetime.datetime.now()),
        ])
        db.commit()

    # Seed Medical Histories
    if db.query(MedicalHistory).count() == 0:
        db.add_all([
            MedicalHistory(patient_id=1, condition="cardiologist", history_details="Has a history of heart conditions."),
            MedicalHistory(patient_id=2, condition="neurologist", history_details="Has a history of neurological disorders."),
            MedicalHistory(patient_id=2, condition="dermatologist", history_details="Has a history of skin conditions."),
        ])
        db.commit()

    # Seed Treatment Machines
    if db.query(TreatmentMachine).count() == 0:
        db.add_all([
            TreatmentMachine(type="Echocardiogram", under_maintenance=False),
            TreatmentMachine(type="MRI Scanner", under_maintenance=False),
            TreatmentMachine(type="Dermascope", under_maintenance=False),
        ])
        db.commit()

    # Seed Treatment Rooms
    if db.query(TreatmentRoom).count() == 0:
        db.add_all([
            TreatmentRoom(name="Cardiology Room", room_type="cardiologist", treatment_machine_id=1),
            TreatmentRoom(name="Neurology Room", room_type="neurologist", treatment_machine_id=2),
            TreatmentRoom(name="Dermatology Room", room_type="dermatologist", treatment_machine_id=3),
            TreatmentRoom(name="General Room 1", room_type=None, treatment_machine_id=None),
            TreatmentRoom(name="General Room 2", room_type=None, treatment_machine_id=None),
        ])
        db.commit()

    db.close()

def main() -> None:
    db = SessionLocal()
    try:
        seed_data(db)
    finally:
        db.close()

if __name__ == "__main__":
    main()
