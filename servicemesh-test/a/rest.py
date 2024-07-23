from fastapi import FastAPI

app = FastAPI()

@app.get("/a/test")
async def get_dates():
    return "from A"


@app.get("/healthz")
def healthcheck():
    return None
