from fastapi import FastAPI
from cachetools import TTLCache
import asyncio
import datetime


resultcache = TTLCache(maxsize=30, ttl=60)
app = FastAPI()

@app.get("/dates")
async def get_dates():
    array = []
    array.append({'Description': 'RDP Birthday', 'Date': datetime.datetime(1991, 6, 20)})
    array.append({'Description' : "Maj Birthday", 'Date': datetime.datetime(1991, 5, 15)})
    array.append({'Description' : "Mor Kirsten Birthday", 'Date': datetime.datetime(1961, 5, 23)})
    array.append({'Description' : "Far Frank Birthday", 'Date': datetime.datetime(1960, 3, 25)})
    array.append({'Description' : "Alberte Birthday", 'Date': datetime.datetime(2017, 3, 4)})
    array.append({'Description' : "Gry Birthday", 'Date': datetime.datetime(2018, 12, 22)})
    array.append({'Description' : "Mathias Birthday", 'Date': datetime.datetime(1992, 4, 24)})
    array.append({'Description' : "Stefan Birthday", 'Date': datetime.datetime(1991, 9, 11)})
    array.append({'Description' : "Wedding Anniversary", 'Date': datetime.datetime(2016, 2, 12)})
    array.append({'Description' : "Kærestedag", 'Date': datetime.datetime(2012, 8, 12)})
    array.append({'Description' : "Ansat ved Trifork", 'Date': datetime.datetime(2019, 1, 3)})
    array.append({'Description' : "Maj Ansat ved VBC", 'Date': datetime.datetime(2021, 7, 1)})
    array.append({'Description' : "Frodos fødselsdag", 'Date': datetime.datetime(2015, 9, 7)})
    array.append({'Description' : "Morten Mikkelsens fødselsdag", 'Date': datetime.datetime(2015, 9, 7)})
    array.append({'Description' : "Idas fødselsdag", 'Date': datetime.datetime(1992, 7, 14)})
    array.append({'Description' : "Bos fødselsdag", 'Date': datetime.datetime(1998, 2, 15)})
    array.append({'Description' : "Ansat ved Bankdata", 'Date': datetime.datetime(2022,12,1)})
    return array
    

@app.get("/healthz")
def healthcheck():
    return None
