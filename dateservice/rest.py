from fastapi import FastAPI
from cachetools import TTLCache
import asyncio
import datetime


resultcache = TTLCache(maxsize=30, ttl=60)
app = FastAPI()


@app.get("/dates")
async def get_dates():
    return [
        {'Description': 'RDP Birthday',
            'Date': datetime.datetime(1991, 6, 20)},
        {'Description': "Maj Birthday",
            'Date': datetime.datetime(1991, 5, 15)},
        {'Description': "Mor Kirsten Birthday",
            'Date': datetime.datetime(1961, 5, 23)},
        {'Description': "Far Frank Birthday",
            'Date': datetime.datetime(1960, 3, 25)},
        {'Description': "Alberte Birthday",
            'Date': datetime.datetime(2017, 3, 4)},
        {'Description': "Gry Birthday",
            'Date': datetime.datetime(2018, 12, 22)},
        {'Description': "Mathias Birthday",
            'Date': datetime.datetime(1992, 4, 24)},
        {'Description': "Stefan Birthday",
            'Date': datetime.datetime(1991, 9, 11)},
        {'Description': "Wedding Anniversary",
            'Date': datetime.datetime(2016, 2, 12)},
        {'Description': "Kærestedag", 'Date': datetime.datetime(2012, 8, 12)},
        {'Description': "Ansat ved Trifork",
            'Date': datetime.datetime(2019, 1, 3)},
        {'Description': "Maj Ansat ved VBC",
            'Date': datetime.datetime(2021, 7, 1)},
        {'Description': "Frodos fødselsdag",
            'Date': datetime.datetime(2015, 9, 7)},
        {'Description': "Morten Mikkelsens fødselsdag",
            'Date': datetime.datetime(2015, 9, 7)},
        {'Description': "Idas fødselsdag",
            'Date': datetime.datetime(1992, 7, 14)},
        {'Description': "Bos fødselsdag",
            'Date': datetime.datetime(1998, 2, 15)},
        {'Description': "Ansat ved Bankdata",
            'Date': datetime.datetime(2022, 12, 1)},
        {'Description': "Frank og Kirsten Bryllupsdag",
            'Date': datetime.datetime(1988, 6, 25)}
    ]


@app.get("/healthz")
def healthcheck():
    return None
