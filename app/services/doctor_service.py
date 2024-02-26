from app.models import Consultation, Doctor, MedicalHistory, TreatmentRoom, TreatmentMachine
from sqlalchemy.orm import Session
from datetime import datetime

class DoctorService:
    def __init__(self, db: Session):
        self.db = db

    def update_availability(self, doctor_id: int, is_available: bool):
        doctor = self.db.query(Doctor).filter(Doctor.id == doctor_id).first()
        if not doctor:
            raise ValueError("Doctor not found")
        doctor.is_available = is_available
        self.db.commit()
        return doctor

    def handle_unavailable_doctor(self, doctor_id: int):
        consultations = self.db.query(Consultation).filter(Consultation.doctor_id == doctor_id, Consultation.start_time > datetime.now()).all()
        for consultation in consultations:
            if self.reassign_consultation(consultation):
                self.notify_patient_reassignment(consultation.patient_id)
            else:
                self.notify_patient_reschedule(consultation.patient_id)

    def reassign_consultation(self, consultation: Consultation) -> bool:
        new_doctor = self.db.query(Doctor).filter(Doctor.specialization == consultation.doctor.specialization, Doctor.is_available == True).first()
        if new_doctor:
            consultation.doctor_id = new_doctor.id
            self.db.commit()
            return True
        return False

    def notify_patient_reassignment(self, patient_id: int):
        print(f"Patient with ID {patient_id} has been reassigned to a new doctor.")

    def notify_patient_reschedule(self, patient_id: int):
        print(f"Patient with ID {patient_id} needs to reschedule their consultation.")
