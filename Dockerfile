FROM python:3.12

WORKDIR /usr/src/app

COPY ./app /usr/src/app/app
COPY ./requirements.txt /usr/src/app/

RUN pip install --no-cache-dir -r requirements.txt

ENV PORT=8000

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "$PORT"]