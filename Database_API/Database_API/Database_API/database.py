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
		self.connect.commit()
		self.connect.close()


	# executes given query
	# returns number of rows affected by query or last rowid
	def raw_query(self, query, rowcount = True):
		try:
			self.open_connection()
			self.cursor.execute(query)
			if rowcount:
				result = self.cursor.rowcount
			else:
				result = self.cursor.lastrowid
			self.close_connetion()
		except:
			result = sys.exc_info()
			print(result)
			print("raw query error")
			print(query)
		return result

db = Database()
db.raw_query("create table testertable(id int, name string)")


