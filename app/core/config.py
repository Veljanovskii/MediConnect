from pydantic_settings import BaseSettings


class Settings(BaseSettings):
    database_url: str = "postgresql://postgres:postgres@localhost/MediConnect"

    class Config:
        env_file = ".env"


settings = Settings()
