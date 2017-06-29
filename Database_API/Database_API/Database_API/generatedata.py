from Database_API import database
import random

def GenerateData():
    db = database.Database()
    #i in range(start, end) (0,500) makes 500 cycles for data generation
    for i in range(0,5000):
        #generate random data
        id = random.randrange(0,200)
        timein = generatetime()
        timeout = generatetime()

def generatetime():
    hour = random.randrange(0,23)
    minute = random.randrange(0,59)
    second = random.randrange(0,59)
    time1 = "{0}:{1}:{2}".format(hour,minute,second)
    return time1, time2

def generatedate():
    year = 2017
    month = random.randrange(0,6)
    if month == 1 or month == 3 or month == 5:
        day = random.randrange(0,31)
    elif month == 2:
        day = random.randrange(0,28)
    else:
        day = random.randrange(0,30)
    date = "{0}:{1}:{2}".format(year,month,day)
    return date





