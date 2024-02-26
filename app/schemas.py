from pydantic import BaseModel, EmailStr, validator
from typing import List, Optional
from datetime import datetime
from enum import Enum


class Specialization(str, Enum):
    cardiologist = "cardiologist"
    neurologist = "neurologist"
    dermatologist = "dermatologist"

# Base models for different entities
class DoctorBase(BaseModel):
    name: str
    email: EmailStr
    specialization: Specialization

class PatientBase(BaseModel):
    name: str
    email: EmailStr

class MedicalHistoryBase(BaseModel):
    history_details: str
    condition: str

class ConsultationBase(BaseModel):
    start_time: datetime
    end_time: datetime
    doctor_id: int
    patient_id: int
    treatment_room_id: int
    is_urgent: Optional[bool] = None

class TreatmentRoomBase(BaseModel):
    id: Optional[int] = None
    name: str
    room_type: Optional[str] = None
    treatment_machine_id: Optional[int] = None
    
    class Config:
        orm_mode = True

class TreatmentMachineBase(BaseModel):
    id: Optional[int] = None
    type: Optional[str] = None
    under_maintenance: Optional[bool] = None

    class Config:
        orm_mode = True

# Models for API input (creation)
class DoctorCreate(DoctorBase):
    pass

class PatientCreate(PatientBase):
    pass

class MedicalHistoryCreate(MedicalHistoryBase):
    patient_id: int

class ConsultationCreate(BaseModel):
    doctor_id: int
    patient_id: int
    treatment_room_id: int
    start_time: datetime
    is_urgent: bool = False

class TreatmentMachineCreate(TreatmentMachineBase):
    pass

class TreatmentRoomCreate(TreatmentRoomBase):
    pass

# Models for API output (reading)
class Doctor(DoctorBase):
    id: int
    consultations: List[ConsultationBase] = []

    class Config:
        orm_mode = True

class Patient(PatientBase):
    id: int
    registration_date: datetime
    medical_history: List[MedicalHistoryBase]

    class Config:
        orm_mode = True

class MedicalHistory(MedicalHistoryBase):
    id: int
    patient_id: int

    class Config:
        orm_mode = True

class TreatmentMachine(TreatmentMachineBase):
    id: int
    treatment_room: Optional[TreatmentRoomBase]

    class Config:
        orm_mode = True

class TreatmentRoom(TreatmentRoomBase):
    id: int
    consultations: List[ConsultationBase] = []
    treatment_machine_id: Optional[int] = None

    class Config:
        orm_mode = True

class Consultation(ConsultationBase):
    id: int
    doctor: Doctor
    patient: Patient
    treatment_room: TreatmentRoom

    class Config:
        orm_mode = True

class ConsultationWithMedicalHistory(BaseModel):    
    consultation: Consultation
    medical_history: List[MedicalHistory]

    class Config:
        orm_mode = True