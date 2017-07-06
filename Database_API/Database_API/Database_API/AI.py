import csv


class matrix():
    def __init__(self, listOfList):
        self.thematrix = listOfList

    def Calculate(self, vector):
        endvector = []
        for i in range(0, len(self.thematrix)):
            endvector.append(0.0)
        for x in range(0, len(vector)):
            for y in range(0, len(self.thematrix)):
                endvector[y] = endvector[y] + vector[x]*self.thematrix[y][x]
        return endvector

class AI():

    def __init__(self, pathToMatrixCSV : str):
        csvfile = open(pathToMatrixCSV, 'r')
        csvTable = csv.reader(csvfile, delimiter=";")

        self.matrices = []

        listoflist = []
        for row in csvTable:
            if(len(row) == 0):
                self.matrices.append(matrix(listoflist))
                listoflist = []
            else:
                listoflist.append([(float)(n) for n in row])

    def Calculate(self, timeInMinutes, rain, vacation):
        vector = [vacation, rain]
        timevector = [0.0 for x in range(0, 96)]
        timeslot = (int)( timeInMinutes/15)
        timevector[timeslot] = 1.0
        vector = vector + timevector

        for currmatrix in self.matrices:
            vector = currmatrix.Calculate(vector)

        highest = 0
        for x in range(0, len(vector)):
            if(vector[x] > vector[highest]):
                highest = x
        if(highest == 0):
            return [0, 30]
        if(highest == 1):
            return [30, 60]
        if(highest == 2):
            return [60, 90]
        

f = AI('matrix.csv')
answer = f.Calculate(675, 1.0, 1.0)


