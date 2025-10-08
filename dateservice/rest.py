from fastapi import FastAPI
import asyncio
import datetime
from fastapi.responses import HTMLResponse

app = FastAPI()

response_404 = """
<!DOCTYPE html>
<html lang="en">
<head>
    <title>Not Found</title>
    
</head>
<body>
    <p>Have you checked your butthole?</p>
    <iframe style="border-radius:12px" src="https://open.spotify.com/embed/track/0yNZ63pUbmxX0xYzZEQr2j?utm_source=generator" width="100%" height="152" frameBorder="0" allowfullscreen="" allow="autoplay; clipboard-write; encrypted-media; fullscreen; picture-in-picture" loading="lazy"></iframe>
    <iframe width="560" height="315" src="https://www.youtube-nocookie.com/embed/--9kqhzQ-8Q?controls=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>
</body>
</html>
"""
    
@app.exception_handler(404)
async def custom_404_handler(_, __):
    return HTMLResponse(response_404)
    
dates = {1:{ 'Description': 'RDP Birthday',
            'Date': datetime.datetime(1991, 6, 20)},
        2:{'Description': "Maj Birthday",
            'Date': datetime.datetime(1991, 5, 15)},
        3:{'Description': "Mor Kirsten Birthday",
            'Date': datetime.datetime(1961, 5, 23)},
        4:{'Description': "Far Frank Birthday",
            'Date': datetime.datetime(1960, 3, 25)},
        5:{'Description': "Alberte Birthday",
            'Date': datetime.datetime(2017, 3, 4)},
        6:{'Description': "Gry Birthday",
            'Date': datetime.datetime(2018, 12, 22)},
        7:{'Description': "Mathias Birthday",
            'Date': datetime.datetime(1992, 4, 24)},
        8:{'Description': "Stefan Birthday",
            'Date': datetime.datetime(1991, 9, 11)},
        9:{'Description': "Wedding Anniversary",
            'Date': datetime.datetime(2016, 2, 12)},
        10:{ 'Description': "Kærestedag", 'Date': datetime.datetime(2012, 8, 12)},
        11:{ 'Description': "Ansat ved Trifork",
            'Date': datetime.datetime(2019, 1, 3)},
        12:{ 'Description': "Maj Ansat ved VBC",
            'Date': datetime.datetime(2021, 7, 1)},
        13:{ 'Description': "Frodos fødselsdag",
            'Date': datetime.datetime(2015, 9, 7)},
        14:{ 'Description': "Morten Mikkelsens fødselsdag",
            'Date': datetime.datetime(1992, 9, 7)},
        15:{ 'Description': "Idas fødselsdag",
            'Date': datetime.datetime(1992, 7, 14)},
        16:{ 'Description': "Bos fødselsdag",
            'Date': datetime.datetime(1998, 2, 15)},
        17:{ 'Description': "Ansat ved Bankdata",
            'Date': datetime.datetime(2022, 12, 1)},
        18:{ 'Description': "Frank og Kirsten Bryllupsdag",
            'Date': datetime.datetime(1988, 6, 25)},
         19:{ 'Description': "Chilis fødselsdag",
            'Date': datetime.datetime(2024, 11, 2)},
         20:{ 'Description': "Chili kom hjem til os",
            'Date': datetime.datetime(2025, 1, 10)},
         21:{ 'Description': "Brunsvigerens dag",
            'Date': datetime.datetime(2025, 10, 9)}
}
@app.get("/dates/{id}")
async def get_date_by_id(id):
    global dates
    return dates[int(id)]

@app.get("/dates")
async def get_dates():
    global dates
    return list(dates.keys())


@app.get("/healthz")
def healthcheck():
    return None
