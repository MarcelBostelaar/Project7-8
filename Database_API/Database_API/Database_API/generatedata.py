from Database_API import database
import random


def GenerateData():
    db = database.Database()
    # i in range(start, end) (0,500) makes 500 cycles for data generation
    for i in range(0, 5):
        wheatherdata = generatedate()
        print("test1")
        if wheatherdata['vakantiedag'] == 1 and wheatherdata['temperature'] > 19 and wheatherdata['regen'] == 0:
            # very low amounts of people in kantine
            tmpint = random.randrange(11, 34)
            for i in range(1, tmpint):
                initials = generateInitials()
                db.postkantinedata(initials['id'], initials['timein'], initials['timeout'], wheatherdata['date'],
                                   wheatherdata['vakantiedag'], wheatherdata['temperature'], wheatherdata['regen'])
        if wheatherdata['vakantiedag'] == 0 and wheatherdata['temperature'] < 18 and wheatherdata['regen'] == 1:
            # very high amounts of people in kantine
            tmpint = random.randrange(32, 78)
            for i in range(1, tmpint):
                initials = generateInitials()
                db.postkantinedata(initials['id'], initials['timein'], initials['timeout'], wheatherdata['date'],
                                   wheatherdata['vakantiedag'], wheatherdata['temperature'], wheatherdata['regen'])
        if wheatherdata['vakantiedag'] == 0 and wheatherdata['temperature'] > 16 and wheatherdata['regen'] == 0:
            # very high amounts of people in kantine
            tmpint = random.randrange(14, 38)
            for i in range(1, tmpint):
                initials = generateInitials()
                db.postkantinedata(initials['id'], initials['timein'], initials['timeout'], wheatherdata['date'],
                                   wheatherdata['vakantiedag'], wheatherdata['temperature'], wheatherdata['regen'])
        if wheatherdata['vakantiedag'] == 1 and wheatherdata['temperature'] < 18 and wheatherdata['regen'] == 0:
            # very high amounts of people in kantine
            tmpint = random.randrange(23, 64)
            for i in range(1, tmpint):
                initials = generateInitials()
                db.postkantinedata(initials['id'], initials['timein'], initials['timeout'], wheatherdata['date'],
                                   wheatherdata['vakantiedag'], wheatherdata['temperature'], wheatherdata['regen'])
        else:
            tmpint = random.randrange(30, 65)
            for i in range(1, tmpint):
                initials = generateInitials()
                db.postkantinedata(initials['id'], initials['timein'], initials['timeout'], wheatherdata['date'],
                                   wheatherdata['vakantiedag'], wheatherdata['temperature'], wheatherdata['regen'])


def generateInitials():
    id = random.randrange(0, 200)
    generatetimes = generatetime()
    timein = generatetimes['timein']
    timeout = generatetimes['timeout']
    return {'id': id, 'timein': timein, 'timeout': timeout}


def generatetime():
    hour = random.randrange(11, 14)
    minute = random.randrange(0, 59)
    second = random.randrange(0, 59)
    # timein == time when the client walks in for lunch
    # time2 == time the client takes for lunching
    # timeout == time when the client leaves the lunch
    timein = "{0}:{1}:{2}".format(hour, minute, second)
    time2 = random.randrange(13, 51)  # 13 to 51 minutes
    if time2 + minute >= 60:
        minute = time2 + minute - 60
        hour = hour + 1
    else:
        minute = time2 + minute
    timeout = "{0}:{1}:{2}".format(hour, minute, second)
    return {'timein': timein, 'timeout': timeout}


def generatedate():
    # defining
    year = 2017
    month = random.randrange(1, 6)
    day = 0
    temp = 0
    regen = 0
    # creating month
    if month == 1 or month == 3:
        day = random.randrange(0, 31)
        temp = random.randrange(0, 15)
    elif month == 2:
        day = random.randrange(0, 28)
        temp = random.randrange(0, 11)
    elif month == 5:
        day = random.randrange(0, 31)
        temp = random.randrange(13, 28)
    elif month == 4:
        day = random.randrange(0, 30)
        temp = random.randrange(7, 17)
    else:
        day = random.randrange(0, 30)
        temp = random.randrange(16, 35)
    # chance of 1 out of 20 for a vakantiedag
    vakantiedag = random.randrange(1, 20)
    if vakantiedag == 19:
        vakantiedag = 1
    else:
        vakantiedag = 0
    # chance of about 1 of 6 for rain
    regen = random.randrange(1, 6)
    if regen == 5:
        regen = 1
    else:
        regen = 0
    date = "{0}:{1}:{2}".format(year, month, day)
    print(date, '\n', vakantiedag, regen, temp)
    return {'date': date, 'temperature': temp, 'vakantiedag': vakantiedag, 'regen': regen}



