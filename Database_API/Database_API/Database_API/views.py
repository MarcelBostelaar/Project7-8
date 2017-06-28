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

@app.route('/db/get/kantinedata', methods=['GET'])
def getkantinedata():
    db = database.Database()
    res = db.getkantinedata()
    return jsonify(res)

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


@app.route('/db/gettest/', methods=['POST'])
def dbgettest():
    print("at dbgettest")
    res = request.data
    print("res == ",res)
    jsonres = request.get_json()
    print("jsonres == ", jsonres["query"])
    db = database.Database()
    return jsonify(db.getrawquery(jsonres["query"]))
    #{"query":"select * from testertable"}


@app.route('/')
@app.route('/home')
def home():
    """Renders the home page."""
    return render_template(
        'index.html',
        title='Home Page',
        year=datetime.now().year,
    )

@app.route('/contact')
def contact():
    """Renders the contact page."""
    return render_template(
        'contact.html',
        title='Contact',
        year=datetime.now().year,
        message='Your contact page.'
    )

@app.route('/about')
def about():
    """Renders the about page."""
    return render_template(
        'about.html',
        title='About',
        year=datetime.now().year,
        message='Your application description page.'
    )
