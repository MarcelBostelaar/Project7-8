"""
This script runs the Database_API application using a development server.
"""

from os import environ
from Database_API import app
from Database_API import database


if __name__ == '__main__':
    HOST = environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(environ.get('SERVER_PORT', '5555'))
    except ValueError:
        PORT = 5555
    app.run(HOST, PORT)
