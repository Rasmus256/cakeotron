from fastapi import FastAPI

app = FastAPI()

@app.get("/c/test")
async def get_dates():
    return "from C"


@app.get("/healthz")
def healthcheck():
    return None
