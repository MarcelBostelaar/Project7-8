"""
Routes and views for the flask application.
"""
import sqlite3
from Database_API import database
from datetime import datetime
from flask import render_template, request
from flask import jsonify
from Database_API import app
from Database_API import AI

#Database
@app.route('/db/get/kantinedata', methods=['GET'])
def getkantinedata():
    db = database.Database()
    res = db.getkantinedata()
    return jsonify(res)
    #returns kantinedata in JSON || gets all kantine data

@app.route('/db/get/kantinedatalast1000', methods=['GET'])
def getkantinedatalast1000():
    db = database.Database()
    res = db.getlast1000kantinedata()
    return jsonify(res)
    #returns kantinedata in JSON || gets all kantine data

@app.route('/db/get/kantinedata1000', methods=['GET'])
def getkantinedata1000():
    db = database.Database()
    res = db.get1000kantinedata()
    return jsonify(res)
    #returns 1000 kantinedata in JSON || gets all kantine data


@app.route('/db/post/kantinedata', methods=['POST'])
def postkantinedata():
    db = database.Database()
    post_data = request.get_json()
    ID = post_data['id']
    timein = post_data['timein']
    timeout = post_data['timeout']
    date = post_data['date']
    vakantiedag = post_data['vakantiedag']
    temperatuur = post_data['temperatuur']
    regen = post_data['regen']
    try:
        result = db.postkantinedata(ID, timein, timeout, date, vakantiedag, temperatuur, regen)
        print(result)
        return str(result), 200
    except:
        return str(result), 500
    #Post example: 
    #{"id":"2",
    # "timein":"19:20:36",
    # "timeout":"19:40:42",
    # "date":"2017:06:24",
    # "vakantiedag":"0",
    # "temperatuur":"21",
    # "regen":"0"
    #}


@app.route('/db/posttest/', methods=['POST'])
def dbposttest():
     jsonres = request.get_json()
     db = database.Database()
     try:
         db.raw_query(jsonres["query"])
         return 200
     except:
         return 500
     #{"query":"create table testtable"}
 
@app.route('/db/gettest/', methods=['POST'])
def dbgettest():
     jsonres = request.get_json()
     print("jsonres == ", jsonres["query"])
     db = database.Database()
     return jsonify(db.getrawquery(jsonres["query"]))
     #{"query":"select * from testertable"}

# get predicted time
@app.route('/getprediction', methods=['POST'])
def AIgetprediction():
    jsondata = request.get_json()
    time = int(jsondata["tijd"])
    vakantiedag = float(jsondata["vakantiedag"])
    regen = float(jsondata["regent"])
    f = AI.AI('matrix.csv')
    answer = f.Calculate(time, regen, vakantiedag)
    print(answer)
    return jsonify(answer)

@app.route('/')
def home():
    return jsonify("Project 7-8 web api")