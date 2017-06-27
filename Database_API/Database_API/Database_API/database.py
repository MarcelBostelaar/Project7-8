import sqlite3
import json
import sys
import os


class Database(object):
    # Contructior opens database
    def __init__(self, databasePath = "Project78.db"):
        self.databasePath = databasePath
        self.select = ""
        self.froms = ""

    def open_connection(self):
        self.connect = sqlite3.connect("Project78.db")

        # this file test use route
        self.cursor = self.connect.cursor()
        self.connect.row_factory = sqlite3.Row

    def close_connetion(self):
        try:
            self.connect.commit()
        except:
            print("Could not commit")
        self.connect.close()

    # executes given query
    # returns number of rows affected by query or last rowid
    def raw_query(self, query, rowcount = True):
        try:
            self.open_connection()
            self.cursor.execute(query)
            if rowcount:
                result = self.cursor.rowcount
                print(result)
            else:
                result = self.cursor.lastrowid
                print(result)
            self.close_connetion()
        except:
            result = sys.exc_info()
            print(result)
            print("raw query error")
            print(query)
        return result

    #Get function from raw query
    #Example: <database>.getraw("select * from testertable")
    def getrawquery(self, query):
        self.open_connection()
        try:
            self.cursor.execute(query)
            result = self.cursor.fetchall()
            print(result)
            return result
        except:
            result = sys.exc_info()
            print(result)
            print("Get query error. \n", query)
            return result
        self.close_connetion()






