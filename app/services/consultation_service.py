from datetime import datetime, timedelta
from sqlalchemy.orm import Session
from app.models import Consultation, Doctor, MedicalHistory, TreatmentRoom, TreatmentMachine
from app.schemas import ConsultationCreate


class ConsultationService:
    def __init__(self, db: Session):
        self.db = db
    
    def create_consultation(self, consultation_data: ConsultationCreate) -> Consultation:
        rounded_start_time = consultation_data.start_time.replace(minute=0, second=0, microsecond=0)
        end_time = rounded_start_time + timedelta(hours=1)

        # Check if the doctor is available
        doctor = self.db.query(Doctor).filter(Doctor.id == consultation_data.doctor_id).first()
        if doctor is None:
            raise ValueError("Doctor not found.")
        if not doctor.is_available:
            raise ValueError("Doctor is not available for consultation.")

        # Check if the time slot is available, unless the consultation is urgent
        if not consultation_data.is_urgent and not self.is_time_slot_available(consultation_data.doctor_id, rounded_start_time, end_time):
            raise ValueError("The time slot is not available.")

        # Check if the doctor's specialization matches the patient's medical condition
        patient_medical_conditions = self.db.query(MedicalHistory.condition).filter(MedicalHistory.patient_id == consultation_data.patient_id).all()
        patient_conditions = [condition[0] for condition in patient_medical_conditions]

        if not any(doctor.specialization.value == condition for condition in patient_conditions):
            raise ValueError("Doctor's specialization does not match patient's medical condition.")

        # Check if the treatment room matches the doctor's specialization
        treatment_room = self.db.query(TreatmentRoom).filter(TreatmentRoom.id == consultation_data.treatment_room_id).first()
        if treatment_room is None:
            raise ValueError("Treatment room not found.")
        if treatment_room.room_type is not None and treatment_room.room_type != doctor.specialization.value:
            raise ValueError("Treatment room does not match doctor's specialization.")

        # Check if the treatment room's machine is under maintenance
        if treatment_room.treatment_machine_id is not None:
            machine = self.db.query(TreatmentMachine).filter(TreatmentMachine.id == treatment_room.treatment_machine_id).first()
            if machine is None or machine.under_maintenance:
                raise ValueError("Treatment room's machine is under maintenance or not found.")

        new_consultation = Consultation(
            doctor_id=consultation_data.doctor_id,
            patient_id=consultation_data.patient_id,
            treatment_room_id=consultation_data.treatment_room_id,
            start_time=rounded_start_time,
            end_time=end_time,
            is_urgent=consultation_data.is_urgent
        )
        self.db.add(new_consultation)
        self.db.commit()
        self.db.refresh(new_consultation)
        return new_consultation


    def is_time_slot_available(self, doctor_id: int, start_time: datetime, end_time: datetime) -> bool:
        overlapping_consultations = self.db.query(Consultation).filter(
            Consultation.doctor_id == doctor_id,
            Consultation.end_time > start_time,
            Consultation.start_time < end_time
        ).first()

        return overlapping_consultations is None
    

    def schedule_control_examination(self, previous_consultation_id: int) -> Consultation:
        previous_consultation = self.db.query(Consultation).filter(Consultation.id == previous_consultation_id).first()
        if previous_consultation is None:
            raise ValueError("Previous consultation not found")

        control_examination_start_time = previous_consultation.start_time + timedelta(weeks=2)
        rounded_start_time = control_examination_start_time.replace(minute=0, second=0, microsecond=0)

        control_examination = Consultation(
            start_time=rounded_start_time,
            end_time=rounded_start_time + timedelta(hours=1),
            doctor_id=previous_consultation.doctor_id,
            patient_id=previous_consultation.patient_id,
            treatment_room_id=previous_consultation.treatment_room_id,
            is_urgent=False
        )
        self.db.add(control_examination)
        self.db.commit()
        self.db.refresh(control_examination)
        return control_examination
