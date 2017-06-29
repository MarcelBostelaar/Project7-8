"""
Routes and views for the flask application.
"""
import sqlite3
from Database_API import database
from datetime import datetime
from flask import render_template, request
from flask import jsonify
from Database_API import app

#Database
@app.route('/db/get/kantinedata', methods=['GET'])
def getkantinedata():
    db = database.Database()
    res = db.getkantinedata()
    return jsonify(res)
    #returns kantinedata in JSON || gets all kantine data

@app.route('/db/post/kantinedata', methods=['POST'])
def postkantinedata():
    db = database.Database()
    post_data = request.get_json()
    ID = post_data['id']
    timein = post_data['timein']
    timeout = post_data['timeout']
    date = post_data['date']
    result = db.postkantinedata(ID, timein, timeout, date)
    return str(result)
    #Post example: 
    #{"id":"2",
    # "timein":"19:20:36",
    # "timeout":"19:40:42",
    # "date":"2017:06:24"
    #}

@app.route('/')
def home():
    return jsonify("Project 7-8 web api")