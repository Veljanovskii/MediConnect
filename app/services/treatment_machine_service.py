from sqlalchemy.orm import Session
from app.models import TreatmentMachine

class TreatmentMachineService:
    def __init__(self, db: Session):
        self.db = db

    def toggle_maintenance_status(self, machine_id: int, new_status: bool):
        machine = self.db.query(TreatmentMachine).filter(TreatmentMachine.id == machine_id).first()
        if machine is None:
            raise ValueError("Treatment machine not found.")

        machine.under_maintenance = new_status
        self.db.commit()
        return machine
