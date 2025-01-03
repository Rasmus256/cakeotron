FROM python:3.14-bookworm

COPY / .

RUN pip install -r ./requirements.txt

CMD ["uvicorn", "rest:app", "--host", "0.0.0.0", "--port", "80"]
