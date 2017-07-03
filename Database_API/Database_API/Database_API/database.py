import sqlite3
import json
import sys
import os
#from Database_API.object import Object

class Database(object):
	# Contructior opens database
	def __init__(self, databasePath = "Database_API\Project78.db"):
		self.databasePath = databasePath
		self.select = ""
		self.froms = ""

	def open_connection(self):
		self.connect = sqlite3.connect("Database_API\Project78.db")

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

	def resetdatabase(self):
		try:
			result = self.cursor.execute("drop table kantinedata")
		except:
			print("no table kantinedata")
		try:
			self.open_connection()
			self.cursor.execute("CREATE TABLE `Kantinedata` (`ID`	string,`timein`	time,`timeout`	time,`date`	date,`vakantiedag`	INTEGER,`temperatuur`	INTEGER,`regen`	INTEGEr);")
			print(result)

		except:
			result = sys.exc_info()
			print(result)
			self.close_connetion()
	#Get all kantinedata
	def getkantinedata(self):
		self.open_connection()
		try:
			self.cursor.execute("select * from kantinedata")
			result = self.cursor.fetchall()
			print(result)
			objects = []
			for data in result:
				obj = Object(data[0],data[1],data[2],data[3],data[4],data[5],data[6])
				objects.append(obj.serialize())
			self.close_connetion()
			return objects
		except:
			pass
			self.close_connection()
		
		
	def postkantinedata(self, id, timein, timeout, date, vakantiedag, temperatuur, regen):
		self.open_connection()
		try:
			self.cursor.execute("INSERT INTO kantinedata(id, timein, timeout, date, vakantiedag, temperatuur, regen) Values({0}, '{1}', '{2}', '{3}', {4}, {5}, {6})".format(id, timein, timeout, date, vakantiedag, temperatuur, regen))
			self.close_connetion()
			return 200
		except:
			result = sys.exc_info()
			print(result)
			self.close_connetion()
			return 400

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





