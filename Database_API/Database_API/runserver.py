"""
This script runs the Database_API application using a development server.
"""

from os import environ
from Database_API import app
from Database_API import database
#from Database_API import generatedata

#db = database.Database()
#db.resetdatabase()


if __name__ == '__main__':
    HOST = environ.get('SERVER_HOST', '145.24.222.31')
    try:
        PORT = int(environ.get('SERVER_PORT', '8080'))
    except ValueError:
        PORT = 8080
    app.run(HOST, PORT)
