FROM  python:3.13.0a3-slim

COPY / .

RUN pip install -r ./requirements.txt

CMD ["uvicorn", "rest:app", "--host", "0.0.0.0", "--port", "80"]
