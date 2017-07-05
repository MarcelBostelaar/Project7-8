import math
import random
from Database_API import database

class entry():

    def __init__(self, rain, vacation, timein, timeout, date):
        self.Rain = rain
        self.Vacation = vacation
        self.TimeIn = timein
        self.TimeOut = timeout
        self.Date = date
        self.Temprature = 21

def generateDataNew():
    db = database.Database()
    temperatuur = random.randrange(0,35)
    id = 1
    alle_entries = []
    for maand in range(1, 13):
        for dag in range(1,29):
            rain = random.randint(0,1)
            vacation = random.randint(0,1)
            for uur in range(1, 24):
                for kwartier in range(1,5):
                    amountOfPeople = (int)(GetBaseAmount(rain, vacation) * GetTimeMultiplier(uur, kwartier*15) * GetDeviationMultiplier())
                    timein = (str)(uur) + ":" + (str)(kwartier * 15 - 10)
                    timeout = (str)(uur) + ":" + (str)(kwartier * 15 - 5)
                    date = "2017:" + (str)(maand) + ":" + (str)(dag)
                    for people in range(0,amountOfPeople):
                        alle_entries.append(entry(rain, vacation, timein, timeout, date))
                        db.postkantinedata(id, timein, timeout, date, vacation, temperatuur, rain)
    return alle_entries
    

    
def GetTimeMultiplier(Hours, Minutes):
    totaltime = Hours*60 + Minutes
    totaltime=totaltime / 60
    result = math.pow(math.e, (-0.5)*(totaltime-12)*(totaltime-12))/ math.sqrt(2*math.pi) * (1/0.4)
    return result

def GetDeviationMultiplier():
    randomfloat = random.randint(-1000, 1000) / 2000
    return math.pow(math.e, (-0.5)*(randomfloat*3)*(randomfloat*3))/ math.sqrt(2*math.pi) * (1/0.4) * -1 + 1.5

def GetBaseAmount(rain, vacation):
    table = [[40,10],[60,20]]
    return table[rain][vacation]

result = generateDataNew()