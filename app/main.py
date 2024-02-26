from datetime import datetime
from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List
from . import crud, models, schemas, dependencies, seed
from .services.consultation_service import ConsultationService
from .services.medical_history_service import MedicalHistoryService
from .services.treatment_machine_service import TreatmentMachineService
from .services.doctor_service import DoctorService
from app.core.database import engine

models.Base.metadata.create_all(bind=engine)

app = FastAPI()

@app.on_event("startup")
def startup_event():
    seed.seed_data()

@app.get("/doctors/", response_model=List[schemas.Doctor])
def read_doctors(skip: int = 0, limit: int = 100, db: Session = Depends(dependencies.get_db)):
    doctors = crud.get_doctors(db, skip=skip, limit=limit)
    return doctors


@app.post("/schedule_consultation/", response_model=schemas.ConsultationWithMedicalHistory)
def schedule_consultation(consultation: schemas.ConsultationCreate, db: Session = Depends(dependencies.get_db)):
    consultation_service = ConsultationService(db)
    medical_history_service = MedicalHistoryService(db)
    try:
        created_consultation = consultation_service.create_consultation(consultation)
        medical_history = medical_history_service.get_medical_history(consultation.patient_id)
        return {
            "consultation": created_consultation, 
            "medical_history": medical_history
        }
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))


@app.post("/medical_history/", response_model=schemas.MedicalHistory)
def add_medical_condition(medical_history: schemas.MedicalHistoryCreate, db: Session = Depends(dependencies.get_db)):
    medical_history_service = MedicalHistoryService(db)
    try:
        return medical_history_service.add_medical_condition(medical_history)
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))


@app.patch("/machines/{machine_id}/maintenance-status", response_model=schemas.TreatmentMachineBase)
def update_machine_maintenance_status(machine_id: int, new_status: bool, db: Session = Depends(dependencies.get_db)):
    machine_service = TreatmentMachineService(db)
    try:
        machine = machine_service.toggle_maintenance_status(machine_id, new_status)
        return machine
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    

@app.patch("/doctors/{doctor_id}/availability")
def update_doctor_availability(doctor_id: int, is_available: bool, db: Session = Depends(dependencies.get_db)):
    doctor_service = DoctorService(db)
    try:
        doctor = doctor_service.update_availability(doctor_id, is_available)
        if not is_available:
            doctor_service.handle_unavailable_doctor(doctor_id)
        return doctor
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))


@app.post("/schedule_control_examination/", response_model=schemas.Consultation)
def schedule_control_examination(previous_consultation_id: int, db: Session = Depends(dependencies.get_db)):
    consultation_service = ConsultationService(db)
    try:
        return consultation_service.schedule_control_examination(previous_consultation_id)
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))
