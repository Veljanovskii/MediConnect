from typing import List
from sqlalchemy.orm import Session
from app.models import MedicalHistory
from app.schemas import MedicalHistoryCreate

class MedicalHistoryService:
    def __init__(self, db: Session):
        self.db = db

    def add_medical_condition(self, medical_history_data: MedicalHistoryCreate) -> MedicalHistory:
        new_medical_history = MedicalHistory(
            patient_id=medical_history_data.patient_id,
            condition=medical_history_data.condition,
            history_details=medical_history_data.history_details
        )
        self.db.add(new_medical_history)
        self.db.commit()
        self.db.refresh(new_medical_history)
        return new_medical_history
    
    def get_medical_history(self, patient_id: int) -> List[MedicalHistory]:
        return self.db.query(MedicalHistory).filter(MedicalHistory.patient_id == patient_id).all()
