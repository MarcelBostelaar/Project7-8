"""
This script runs the Database_API application using a development server.
"""

from os import environ
from Database_API import app
from Database_API import database


if __name__ == '__main__':
    HOST = environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(environ.get('SERVER_PORT', '8080'))
    except ValueError:
        PORT = 8080
    app.run(HOST, PORT)
