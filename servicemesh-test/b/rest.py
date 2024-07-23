from fastapi import FastAPI

app = FastAPI()

@app.get("/b/test")
async def get_dates():
    return "from B"


@app.get("/healthz")
def healthcheck():
    return None
